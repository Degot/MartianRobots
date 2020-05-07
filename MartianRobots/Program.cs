using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using MartianRobots.Common;
using MartianRobots.Common.Interfaces;

namespace MartianRobots
{
    class Program
    {
        static void Main(string[] args)
        {
            //Load implementations of IGrid and IRobotInstructionHandler from currentFolder
            var folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var grids = LoadImplementation<IGrid>(folder);
            if (grids == null || grids.Length == 0)
            {
                Console.WriteLine("No IGrid implementations were found");
                return;
            }

            //Each instruction has unique char Key.
            var instructionHandlers = LoadImplementation<IRobotInstructionHandler>(folder);
            if (instructionHandlers == null || instructionHandlers.Length == 0)
            {
                Console.WriteLine("No IRobotInstructionHandler implementations were found.");
                return;
            }

            if (instructionHandlers.Select(rc => rc.Key).Distinct().Count() != instructionHandlers.Length)
            {
                Console.WriteLine("There are multiple IRobotInstructionHandlers for the same instruction.");
                return;
            }

            //SampleCommands.txt is used as input
            using (var world = new World(grids[0], new RobotFactory(), instructionHandlers))
            {
                using (var stream = new FileStream("SampleCommands.txt", FileMode.Open, FileAccess.Read, FileShare.Read))
                using (var sr = new StreamReader(stream))
                {
                    var line = string.Empty;

                    while ((line = sr.ReadLine()) != null)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                            world.ProcessInput(line);
                    }
                }

                //Output results into console.
                foreach (var robot in world.Robots)
                {
                    Console.WriteLine($"{robot.Position.X} {robot.Position.Y} {robot.Orientation}" + (robot.Lost ? " LOST" : ""));
                }
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey(true);

        }


        private static T[] LoadImplementation<T>(string folder)
        {
            var interfaceType = typeof(T);
            if (string.IsNullOrEmpty(folder) || !Directory.Exists(folder))
            {
                return null;
            }

            var implementations = new List<T>();
            foreach (var filename in Directory.GetFiles(folder, "*.dll", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    var currentAssembly = Assembly.LoadFile(filename);
                    var types = currentAssembly.GetTypes();
                    foreach (var type in types)
                    {
                        if (!((type != interfaceType && interfaceType.IsAssignableFrom(type))))
                        {
                            continue;
                        }

                        implementations.Add((T)Activator.CreateInstance(type));
                    }

                }
                catch (Exception ex)
                {
                    continue;
                }
            }

            return implementations.ToArray();
        }
    }
}
