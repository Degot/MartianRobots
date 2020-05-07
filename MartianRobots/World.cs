using System;
using System.Collections.Generic;
using System.Linq;
using MartianRobots.Common;
using MartianRobots.Common.Domain;
using MartianRobots.Common.Interfaces;

namespace MartianRobots
{
    internal partial class World : IDisposable
    {
        private readonly StateMachine _brain; //Process input based on state
        private readonly IGrid _grid;
        private readonly IRobotFactory _robotFactory;
        private readonly List<Robot> _robots;
        private readonly Dictionary<char, IRobotInstructionHandler> _dispatcher; 

        private readonly Parser _parser; //Input line parser
        public IEnumerable<Robot> Robots => _robots;

        public World(IGrid grid, IRobotFactory robotFactory, IRobotInstructionHandler[] instructionHandlers)
        {
            _brain = new StateMachine();
            _grid = grid;
            _robotFactory = robotFactory;
            _robots = new List<Robot>();

            _dispatcher = instructionHandlers.ToDictionary(rc => rc.Key, rc => rc);
            _parser = new Parser(instructionHandlers);

            _brain.SetState(InitializeGrid);
        }

        private void InitializeGrid(string line)
        {
            var commandDefinition = _parser.Parse(line, true)[0];
            
            _grid.Reset();
            _grid.Init(commandDefinition);
            _brain.SetState(CreateAndPlaceRobot);
        }

        private void CreateAndPlaceRobot(string line)
        {
            var args = _parser.Parse(line, true)[0].Arguments;
            var robot = _robotFactory.CreateRobot(args);
            _robots.Add(robot);

            if (!_grid.IsValidPosition(robot.Position))
            {
                robot.Lost = true;
            }

            _brain.SetState(ProcessRobotInstructions);
        }
        private void ProcessRobotInstructions(string line)
        {
            var robot = _robots.Last();

            var commands = _parser.Parse(line, false);


            foreach (var command in commands)
            {
                if (!robot.Lost && !_grid.HasScent(robot, command))
                {
                    var newRobot = _dispatcher[command.Key].Execute(robot);

                    if (_grid.IsValidPosition(newRobot.Position))
                    {
                        robot.Apply(newRobot);
                    }
                    else
                    {
                        robot.Lost = true;
                        _grid.AddScent(robot, command);
                        break;
                    }
                }
                
            }
            _brain.SetState(CreateAndPlaceRobot);
        }
        public void ProcessInput(string line)
        {
            _brain.Update(line);
        }
        

        private class StateMachine
        {
            private Action<string> _activeState;

            public void SetState(Action<string> state)
            {
                _activeState = state;
            }

            public void Update(string line)
            {
                _activeState?.Invoke(line);
            }
        }

        public void Dispose()
        {
            _parser?.Dispose();
        }
    }
}