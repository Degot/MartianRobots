using System;
using System.Collections.Generic;
using MartianRobots.Common.Domain;

namespace MartianRobots.Common.Utils
{
    public class OrientationManager
    {
        //Position  Deltas for orientations in CW order
        private static readonly List<KeyValuePair<string, Position>> _orientationValues = new List<KeyValuePair<string, Position>>()
        {
            new KeyValuePair<string, Position>("N", new Position(0, 1)),
            new KeyValuePair<string, Position>("E", new Position(1, 0)),
            new KeyValuePair<string, Position>("S", new Position(0, -1)),
            new KeyValuePair<string, Position>("W", new Position(-1, 0)),
        };
        private static readonly Lazy<OrientationManager> _instance = new Lazy<OrientationManager>(() => new OrientationManager());
        public static OrientationManager Instance => _instance.Value;
        private OrientationManager() { }

        public bool IsValid(string orientation)
        {
            return GetIndexByOrientation(orientation) != -1;
        }
        

        private int GetIndexByOrientation(string orientation)
        {
            var orientationIndex = -1;
            for (var i = 0; i < _orientationValues.Count; ++i)
            {
                if (_orientationValues[i].Key == orientation)
                {
                    orientationIndex = i;
                    break;
                }
            }

            return orientationIndex;
        }

        private string GetOrientationByIndex(int index)
        {
            var count = _orientationValues.Count;

            var l_index = index;
            if (l_index < 0)
            {
                l_index = ((l_index % count) + count) % count;
            }
            else
            {
                l_index = l_index % count;
            }

            return _orientationValues[l_index].Key;

        }
        public string TurnRight(string orientation)
        {
            return GetOrientationByIndex(GetIndexByOrientation(orientation) + 1);
        }

        public string TurnLeft(string orientation)
        {

            return GetOrientationByIndex(GetIndexByOrientation(orientation) - 1);
        }

        public Position GetPositionDelta(string orientation)
        {
            return _orientationValues[GetIndexByOrientation(orientation)].Value;
        }

    }
}
