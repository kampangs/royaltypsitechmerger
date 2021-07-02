using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Royalty_PsiTech_Merger
{
    [StaticConstructorOnStartup]
    public sealed class Royalty_PsiTech_Merger_Starter : Mod
    {
        public Royalty_PsiTech_Merger_Starter(ModContentPack content) : base(content)
        {
            var harmony = new Harmony("iron_xides.royalty_psitech_merger");
            harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(Reward_Items), "InitFromValue")]
    class NoPsylinkNeuroformersAsQuestRewardsPatch
    {
        static void Prefix(ref RewardsGeneratorParams parms)
        {
            if(parms.disallowedThingDefs == null)
            {
                parms.disallowedThingDefs = new List<ThingDef>();
            }
            parms.disallowedThingDefs.Add(ThingDefOf.PsychicAmplifier);
        }
    }

    [HarmonyPatch(typeof(NeurotrainerDefGenerator), "ImpliedThingDefs")]
    class NoPsitrainersFromTradersOrQuestRewardsPatch
    {
        static IEnumerable<ThingDef> Postfix(IEnumerable<ThingDef> __result)
        {
            foreach (var def in __result)
            {
                if (def.IsWithinCategory(ThingCategoryDefOf.NeurotrainersPsycast))
                {
                    def.tradeability = Tradeability.Sellable;
                    // "thingSetMaker" means quest rewards
                    if (def.thingSetMakerTags != null)
                    {
                        def.thingSetMakerTags.Clear();
                    }
                }
                yield return def;
            }
        }
    }

    [HarmonyPatch(typeof(CompMeditationFocus), "SpecialDisplayStats")]
    class HideMeditationStatsCategoryOnBuildings
    {
        static bool Prefix(ref IEnumerable<StatDrawEntry> __result)
        {
            return false;
        }
    }

    [HarmonyPatch(typeof(Pawn), "SpecialDisplayStats")]
    class HideMeditationFocusTypesOnPawns
    {
        static IEnumerable<StatDrawEntry> Postfix(IEnumerable<StatDrawEntry> __result)
        {
            foreach(StatDrawEntry entry in __result)
            {
                // Meditation Focuses display priority = 99995
                if(entry.DisplayPriorityWithinCategory == 99995)
                {
                    yield break;
                }
                else
                {
                    yield return entry;
                }
            }
        }
    }

}
