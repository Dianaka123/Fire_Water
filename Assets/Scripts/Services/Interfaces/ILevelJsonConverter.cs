﻿using Assets.Scripts.Configs;

namespace Assets.Scripts.Services.Interfaces
{
    public interface ILevelJsonConverter
    {
        string SerializeLevel(Level level);
        Level DeserializeLevel(string txt);
        Level[] DeserializeAllLevels(string txt);
    }
}
