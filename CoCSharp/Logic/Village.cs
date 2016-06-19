﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a Clash of Clans village.
    /// </summary>
    public class Village
    {
        //TODO: A new JSON field was introduced in 8.x.x called "id" which represents the instance id of a village object.
        //TODO: Implement VillageObjectCollection class.

        private const int TownHallDataID = 1000001;

        /// <summary>
        /// Represents the width of a <see cref="Village"/> layout.
        /// </summary>
        public const int Width = 48;

        /// <summary>
        /// Represents the height of a <see cref="Village"/> layout.
        /// </summary>
        public const int Height = 48;

        /// <summary>
        /// Initializes a new instance of the <see cref="Village"/> class.
        /// </summary>
        public Village()
        {
            Buildings = new List<Building>();
            Obstacles = new List<Obstacle>();
            Traps = new List<Trap>();
            Decorations = new List<Decoration>();
        }

        /// <summary>
        /// Gets or sets the experience version? (Not completely sure if thats its full name).
        /// </summary>
        /// <remarks>
        /// I don't know what this is needed for but I found it in the 8.x.x update
        /// and the client needs it when there is a "loot_multiplier_ver" in an Obstacle object; it crashes
        /// if it does not find it.
        /// </remarks>
        public int ExperienceVersion { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="Building"/> in the <see cref="Village"/>.
        /// </summary>
        public List<Building> Buildings { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="Obstacle"/> in the <see cref="Village"/>.
        /// </summary>
        public List<Obstacle> Obstacles { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="Trap"/> in the <see cref="Village"/>.
        /// </summary>
        public List<Trap> Traps { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="Decoration"/> in the <see cref="Village"/>.
        /// </summary>
        public List<Decoration> Decorations { get; set; }

        /// <summary>
        /// Gets or sets the TownHall <see cref="Building"/> of the <see cref="Village"/>; returns
        /// null if there is no TownHall in the <see cref="Village"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> Data ID is not 1000001.</exception>
        public Building TownHall
        {
            get
            {
                for (int i = 0; i < Buildings.Count; i++)
                {
                    var building = Buildings[i];
                    if (building.GetDataID() == TownHallDataID)
                        return building;
                }

                return null;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                if (value.GetDataID() != TownHallDataID)
                    throw new ArgumentException("value must be a TownHall building, that is a building with Data ID '1000001'.");

                // Look for a TownHall Building in the village
                // If we find it, we change its reference to value.
                for (int i = 0; i < Buildings.Count; i++)
                {
                    var building = Buildings[i];
                    if (building.GetDataID() == TownHallDataID)
                    {
                        // Exit early so it ignores the Building.Add below.
                        Buildings[i] = value;
                        return;
                    }
                }

                // If we did not find any TownHall we add it to the village.
                Buildings.Add(value);
            }
        }

        /// <summary>
        /// Gets the JSON string from which the <see cref="Village"/> was
        /// deserialized; returns <c>null</c> if the <see cref="Village"/> wasn't deserialized.
        /// </summary>
        public string DeserializedJson { get; private set; }

        #region Potential Private Methods

        // These methods becomes pointless to be public be cause
        // of the GetVillageObject<T>(gameId) method.

        /// <summary>
        /// Gets the <see cref="Building"/> with the specified game ID.
        /// </summary>
        /// <param name="gameId"><see cref="Building"/> with the game ID.</param>
        /// <returns><see cref="Building"/> with the specified game ID.</returns>
        /// <exception cref="ArgumentException"><paramref name="gameId"/> is not a valid <see cref="Building"/> game ID.</exception>
        /// <exception cref="ArgumentException"><paramref name="gameId"/> is not in the <see cref="Village"/>.</exception>
        public Building GetBuilding(int gameId)
        {
            if (gameId < Building.BaseGameID || gameId > Building.BaseGameID + VillageObject.Base)
                throw new ArgumentException("'" + gameId + "' is not a valid Building game ID. It must be between '" + Building.BaseGameID +
                                            "' and '" + (Building.BaseGameID + VillageObject.Base) + "'.");

            var index = gameId - Building.BaseGameID;
            // Check if the index is valid.
            // Index should be never less than 0 as it was checked above.
            if (index >= Buildings.Count)
                throw new ArgumentException("Could not find Building with game ID '" + gameId + "'.", "gameId");

            return Buildings[index];
        }

        /// <summary>
        /// Gets the <see cref="Obstacle"/> with the specified game ID.
        /// </summary>
        /// <param name="gameId"><see cref="Obstacle"/> with the game ID.</param>
        /// <returns><see cref="Obstacle"/> with the specified game ID.</returns>
        /// <exception cref="ArgumentException"><paramref name="gameId"/> is not a valid <see cref="Obstacle"/> game ID.</exception>
        /// <exception cref="ArgumentException"><paramref name="gameId"/> is not in the <see cref="Village"/>.</exception>
        public Obstacle GetObstacle(int gameId)
        {
            if (gameId < Obstacle.BaseGameID || gameId > Obstacle.BaseGameID + VillageObject.Base)
                throw new ArgumentException("'" + gameId + "' is not a valid Obstacle game ID. It must be between '" + Obstacle.BaseGameID +
                                            "' and '" + (Obstacle.BaseGameID + VillageObject.Base) + "'.");

            var index = gameId - Obstacle.BaseGameID;
            // Check if the index is valid.
            // Index should be never less than 0 as it was checked above.
            if (index >= Obstacles.Count)
                throw new ArgumentException("Could not find Obstacle with game ID '" + gameId + "'.", "gameId");

            return Obstacles[index];
        }

        /// <summary>
        /// Gets the <see cref="Trap"/> with the specified game ID.
        /// </summary>
        /// <param name="gameId"><see cref="Trap"/> with the game ID.</param>
        /// <returns><see cref="Trap"/> with the specified game ID.</returns>
        /// <exception cref="ArgumentException"><paramref name="gameId"/> is not a valid <see cref="Trap"/> game ID.</exception>
        /// <exception cref="ArgumentException"><paramref name="gameId"/> is not in the <see cref="Village"/>.</exception>
        public Trap GetTrap(int gameId)
        {
            if (gameId < Trap.BaseGameID || gameId > Trap.BaseGameID + VillageObject.Base)
                throw new ArgumentException("'" + gameId + "' is not a valid Trap game ID. It must be between '" + Trap.BaseGameID +
                                            "' and '" + (Trap.BaseGameID + VillageObject.Base) + "'.");

            var index = gameId - Trap.BaseGameID;
            // Check if the index is valid.
            // Index should be never less than 0 as it was checked above.
            if (index >= Traps.Count)
                throw new ArgumentException("Could not find Trap with game ID '" + gameId + "'.", "gameId");

            return Traps[index];
        }

        /// <summary>
        /// Gets the <see cref="Decoration"/> with the specified game ID.
        /// </summary>
        /// <param name="gameId"><see cref="Decoration"/> with the game ID.</param>
        /// <returns><see cref="Decoration"/> with the specified game ID.</returns>
        /// <exception cref="ArgumentException"><paramref name="gameId"/> is not a valid <see cref="Decoration"/> game ID.</exception>
        /// <exception cref="ArgumentException"><paramref name="gameId"/> is not in the <see cref="Village"/>.</exception>
        public Decoration GetDecoration(int gameId)
        {
            if (gameId < Decoration.BaseGameID || gameId > Decoration.BaseGameID + VillageObject.Base)
                throw new ArgumentException("'" + gameId + "' is not a valid Decoration game ID. It must be between '" + Decoration.BaseGameID +
                                            "' and '" + (Decoration.BaseGameID + VillageObject.Base) + "'.");

            var index = gameId - Decoration.BaseGameID;
            // Check if the index is valid.
            // Index should be never less than 0 as it was checked above.
            if (index >= Decorations.Count)
                throw new ArgumentException("Could not find Decoration with game ID '" + gameId + "'.", "gameId");

            return Decorations[index];
        }

        #endregion

        /// <summary>
        /// Gets the <see cref="VillageObject"/> with specified game ID as 
        /// the specified <see cref="VillageObject"/> type.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="VillageObject"/> to return.</typeparam>
        /// <param name="gameId"><see cref="VillageObject"/> with the game ID.</param>
        /// <returns><see cref="VillageObject"/> with the specified game ID.</returns>
        public T GetVillageObject<T>(int gameId) where T : VillageObject
        {
            return (T)GetVillageObject(gameId);
        }

        /// <summary>
        /// Gets the <see cref="VillageObject"/> with the specified game ID; if not <see cref="VillageObject"/>
        /// with the same game ID is found returns null.
        /// </summary>
        /// <param name="gameId"><see cref="VillageObject"/> with the game ID.</param>
        /// <returns><see cref="VillageObject"/> with the specified game ID.</returns>
        /// <exception cref="ArgumentException"><paramref name="gameId"/> is not in the village.</exception>
        public VillageObject GetVillageObject(int gameId)
        {
            // Code repetition here with double checking of gameIds.
            if (gameId >= Building.BaseGameID && gameId < Building.BaseGameID + VillageObject.Base)
                return GetBuilding(gameId);
            else if (gameId >= Obstacle.BaseGameID && gameId < Obstacle.BaseGameID + VillageObject.Base)
                return GetObstacle(gameId);
            else if (gameId >= Trap.BaseGameID && gameId < Trap.BaseGameID + VillageObject.Base)
                return GetTrap(gameId);
            else if (gameId >= Decoration.BaseGameID && gameId < Decoration.BaseGameID + VillageObject.Base)
                return GetDecoration(gameId);
            else
                return null;
        }

        /// <summary>
        /// Returns a non-indented JSON string that represents the current <see cref="Village"/>.
        /// </summary>
        /// <returns>A non-indented JSON string that represents the current <see cref="Village"/>.</returns>
        public string ToJson()
        {
            return ToJson(false);
        }

        /// <summary>
        /// Returns a JSON string and indented if specified that represents the current <see cref="Village"/>.
        /// </summary>
        /// <param name="indent">If set to <c>true</c> the returned JSON string will be indented.</param>
        /// <returns>A JSON string and indented if specified that represents the current <see cref="Village"/>.</returns>
        public string ToJson(bool indent)
        {
            var jsonStr = string.Empty;

            using (var textWriter = new StringWriter())
            using (var jsonWriter = new JsonTextWriter(textWriter))
            {
                if (indent)
                {
                    jsonWriter.Formatting = Formatting.Indented;
                    jsonWriter.Indentation = 4;
                }

                jsonWriter.WriteStartObject();

                jsonWriter.WritePropertyName("exp_ver");
                jsonWriter.WriteValue(ExperienceVersion);

                jsonWriter.WritePropertyName("buildings");
                WriteBuildingArray(jsonWriter);

                jsonWriter.WritePropertyName("obstacles");
                WriteObstacleArray(jsonWriter);

                jsonWriter.WritePropertyName("traps");
                WriteTrapArray(jsonWriter);

                jsonWriter.WritePropertyName("decos");
                WriteTrapArray(jsonWriter);

                jsonWriter.WriteEndObject();

                jsonStr = textWriter.ToString();
            }

            return jsonStr;
        }

        /// <summary>
        /// Returns a <see cref="Village"/> that will be deserialize from the specified
        /// JSON string.
        /// </summary>
        /// <param name="value">JSON string that represents the <see cref="Village"/>.</param>
        /// <returns>A <see cref="Village"/> that is deserialized from the specified JSON string.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null or empty.</exception>
        public static Village FromJson(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("value");

            var village = new Village();

            using (var textReader = new StringReader(value))
            using (var jsonReader = new JsonTextReader(textReader))
            {
                while (jsonReader.Read())
                {
                    if (jsonReader.TokenType == JsonToken.PropertyName)
                    {
                        var propertyName = (string)jsonReader.Value;
                        switch (propertyName)
                        {
                            case "exp_ver":
                                village.ExperienceVersion = jsonReader.ReadAsInt32().Value;
                                break;

                            case "buildings":
                                ReadBuildingArray(jsonReader, village);
                                break;

                            case "obstacles":
                                ReadObstacleArray(jsonReader, village);
                                break;

                            case "traps":
                                ReadTrapArray(jsonReader, village);
                                break;

                            case "decos":
                                ReadDecorationArray(jsonReader, village);
                                break;
                        }
                    }
                }
            }

            village.DeserializedJson = value;

            // Schedule constructions of Village Objects so that
            // it executes logic.
            ScheduleVillageObjects(village);

            return village;
        }

        private void WriteBuildingArray(JsonWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            writer.WriteStartArray();
            for (int i = 0; i < Buildings.Count; i++)
            {
                var building = Buildings[i];
                building.ToJsonWriter(writer);
            }
            writer.WriteEndArray();
        }

        private void WriteObstacleArray(JsonWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            writer.WriteStartArray();
            for (int i = 0; i < Obstacles.Count; i++)
            {
                var obstacle = Obstacles[i];
                obstacle.ToJsonWriter(writer);
            }
            writer.WriteEndArray();
        }

        private void WriteTrapArray(JsonWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            writer.WriteStartArray();
            for (int i = 0; i < Traps.Count; i++)
            {
                var trap = Traps[i];
                trap.ToJsonWriter(writer);
            }
            writer.WriteEndArray();
        }

        private void WriteDecorationArray(JsonWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            writer.WriteStartArray();
            for (int i = 0; i < Decorations.Count; i++)
            {
                var decoration = Decorations[i];
                decoration.ToJsonWriter(writer);
            }
            writer.WriteEndArray();
        }

        private static void ReadBuildingArray(JsonReader reader, Village village)
        {
            // Make sure we are in an array first.
            if (!reader.Read() || reader.TokenType != JsonToken.StartArray)
                throw new JsonException("Expected a JSON start array.");

            var building = (Building)null;
            while (reader.Read())
            {
                // Break when we read the end of the array.
                if (reader.TokenType == JsonToken.EndArray)
                    break;

                if (reader.TokenType == JsonToken.StartObject)
                {
                    building = new Building();
                    building.FromJsonReader(reader);
                    village.Buildings.Add(building);

                    building = null;
                }
            }
        }

        private static void ReadObstacleArray(JsonReader reader, Village village)
        {
            // Make sure we are in an array first.
            if (!reader.Read() || reader.TokenType != JsonToken.StartArray)
                throw new JsonException("Expected a JSON start array.");

            var obstacle = (Obstacle)null;
            while (reader.Read())
            {
                // Break when we read the end of the array.
                if (reader.TokenType == JsonToken.EndArray)
                    break;

                if (reader.TokenType == JsonToken.StartObject)
                {
                    obstacle = new Obstacle();
                    obstacle.FromJsonReader(reader);
                    village.Obstacles.Add(obstacle);

                    obstacle = null;
                }
            }
        }

        private static void ReadTrapArray(JsonReader reader, Village village)
        {
            // Make sure we are in an array first.
            if (!reader.Read() || reader.TokenType != JsonToken.StartArray)
                throw new JsonException("Expected a JSON start array.");

            var trap = (Trap)null;
            while (reader.Read())
            {
                // Break when we read the end of the array.
                if (reader.TokenType == JsonToken.EndArray)
                    break;

                if (reader.TokenType == JsonToken.StartObject)
                {
                    trap = new Trap();
                    trap.FromJsonReader(reader);
                    village.Traps.Add(trap);

                    trap = null;
                }
            }
        }

        private static void ReadDecorationArray(JsonReader reader, Village village)
        {
            // Make sure we are in an array first.
            if (!reader.Read() || reader.TokenType != JsonToken.StartArray)
                throw new JsonException("Expected a JSON start array.");

            var decoration = (Decoration)null;
            while (reader.Read())
            {
                // Break when we read the end of the array.
                if (reader.TokenType == JsonToken.EndArray)
                    break;

                if (reader.TokenType == JsonToken.StartObject)
                {
                    decoration = new Decoration();
                    decoration.FromJsonReader(reader);
                    village.Decorations.Add(decoration);

                    decoration = null;
                }
            }
        }

        private static void ScheduleVillageObjects(Village village)
        {
            for (int i = 0; i < village.Buildings.Count; i++)
            {
                var building = village.Buildings[i];

                // If the building is in construction, schedule it with its current loaded construction time.
                if (building.IsConstructing)
                    building.InternalScheduleBuild();
            }

            for (int i = 0; i < village.Traps.Count; i++)
            {
                var trap = village.Traps[i];

                // If the trap is in construction, schedule it with its current loaded construction time.
                if (trap.IsConstructing)
                    trap.InternalScheduleBuild();
            }

            for (int i = 0; i < village.Obstacles.Count; i++)
            {
                var obstacle = village.Obstacles[i];

                // If the obstacle is being cleared, schedule it with its current loaded clear time.
                if (obstacle.IsClearing)
                    obstacle.InternalScheduleClearing();
            }

            // No need to schedule decorations because their constructions are instant.
        }
    }
}
