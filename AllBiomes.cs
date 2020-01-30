using Pipliz.JSON;
using Science;
using System.Collections.Generic;

namespace grasmanek94.AllBiomes
{
    [ModLoader.ModManager]
    public static class AllBiomes
    {
        static List<ScienceKey> sciencebiomes;

        static void Initialize()
        {
            if(sciencebiomes != null && sciencebiomes.Count > 0)
            {
                return;
            }

            List<string> biomes = new List<string>
            {
                "biome.oldworld",
                "biome.newworld",
                "biome.fareast",
                "biome.tropics",
                "biome.arctic"
            };

            sciencebiomes = new List<ScienceKey>();

            if (ServerManager.ScienceManager == null)
            {
                return;
            }

            foreach (var biome in biomes)
            {
                ScienceKey key = ServerManager.ScienceManager.GetKey(biome);
                if (key.Researchable != null)
                {
                    sciencebiomes.Add(key);
                }
            }
        }

        static void ResearchBiomes(Colony colony)
        {
            if ( colony == null || 
                colony.ScienceData == null )
            {
                return;
            }

            Initialize();

            foreach (var sciencebiome in sciencebiomes)
            {
                if (colony.ScienceData.CompletedScience != null)
                {
                    colony.ScienceData.CompletedScience.AddIfUnique(sciencebiome);
                }

                if (colony.ScienceData.ScienceMask != null)
                {
                    colony.ScienceData.ScienceMask.SetAvailable(sciencebiome, true);
                }
            }
        }

        [ModLoader.ModCallback(ModLoader.EModCallbackType.OnLoadingColony, "OnLoadingColony")]
        static void OnLoadingColony(Colony colony, JSONNode node)
        {
            ResearchBiomes(colony);
        }

        [ModLoader.ModCallback(ModLoader.EModCallbackType.OnCreatedColony, "OnCreatedColony")]
        static void OnCreatedColony(Colony colony)
        {
            ResearchBiomes(colony);
        }

        [ModLoader.ModCallback(ModLoader.EModCallbackType.OnActiveColonyChanges, "OnActiveColonyChanges")]
        static void OnActiveColonyChanges(Players.Player player, Colony oldColony, Colony newColony)
        {
            ResearchBiomes(oldColony);
            ResearchBiomes(newColony);
        }

        [ModLoader.ModCallback(ModLoader.EModCallbackType.OnPlayerRespawn, "OnPlayerRespawn")]
        static void OnPlayerRespawn(Players.Player player)
        {
            ResearchBiomes(player.ActiveColony);
        }
    }
}
