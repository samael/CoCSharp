﻿using CoCSharp.Database.Csv;
using CoCSharp.Logic;
using System.Collections.Generic;
using System.Linq;

namespace CoCSharp.Database
{
    public class ObstacleDatabase : BaseDatabase
    {
        #region Columns
        private const int NameColumn = 0;
        private const int ClearResource = 11;
        private const int ClearCost = 12;
        private const int LootResource = 13;
        private const int LootCount = 14;
        private const int RespawnWeight = 17;
        #endregion

        public const int ID = 8000000;

        public ObstacleDatabase(string path) : base(path)
        {
            this.Obstacles = new List<Obstacle>();
        }

        public List<Obstacle> Obstacles { get; set; }

        public bool TryGetObstacle(int id, out Obstacle obstacle)
        {
            obstacle = Obstacles.Where(d => d.ID == id).FirstOrDefault();
            return obstacle == null ? false : true;
        }

        public bool TryGetObstacle(string name, out Obstacle obstacle)
        {
            obstacle = Obstacles.Where(d => d.Name == name).FirstOrDefault();
            return obstacle == null ? false : true;
        }


        public override void LoadDatabase()
        {
            var id = ID;     
            while (!CsvTable.EndOfFile)
            {
                var row = CsvTable.ReadNextRow();

                var name = row.GetRecord(NameColumn);
                var clearResource = row.GetRecordAsResource(ClearResource);
                var clearCost = row.GetRecordAsInt(ClearCost);
                var lootResource = row.GetRecordAsResource(LootResource);
                var lootCount = row.GetRecordAsInt(LootCount);
                var respawnWeight = row.GetRecordAsInt(RespawnWeight);

                var obstacle = new Obstacle(id);
                obstacle.Name = name;
                obstacle.ClearResource = clearResource;
                obstacle.ClearCost = clearCost;
                obstacle.LootResource = lootResource;
                obstacle.LootCount = lootCount;
                obstacle.RespawnWeight = respawnWeight;

                Obstacles.Add(obstacle);
                id++;
            }
        }
    }
}