using System.Collections.Generic;
using Verse;
using RimWorld;
using PsiTech.Psionics;
using PsiTech.Utility;

namespace Royalty_PsiTech_Merger
{
    static class BlindPsiTrainingUtility
    {
        static void AddToPool(List<PsiTechAbilityDef> abilityPool, string abilityDefName, PsiTechTracker psitracker)
        {
            PsiTechAbilityDef abilityDef = DefDatabase<PsiTechAbilityDef>.GetNamed(abilityDefName);
            if(!psitracker.HasAbility(abilityDef))
            {
                abilityPool.Add(abilityDef);
            }
        }
        static public void Train(Pawn trainee)
        {
            PsiTechTracker psitracker = trainee.PsiTracker();
            if (psitracker == null) return;
            // Awaken pawn if not awakened
            if (!psitracker.Activated)
            {
                psitracker.ActivateTracker();
            }
            // If already awakened, give a focus or energy node
            else
            {
                if (psitracker.FocusLevel < 3 && (Rand.Bool || psitracker.EnergyLevel >= 3))
                {
                    psitracker.FocusLevel += 1;
                }
                else if (psitracker.EnergyLevel < 3)
                {
                    psitracker.EnergyLevel += 1;
                }
            }
            // Regardless, give a tier 1 ability if possible
            if (psitracker.HasAvailableSlot(1))
            {
                List<PsiTechAbilityDef> ability_pool = new List<PsiTechAbilityDef>();

                // Only provide T1 abilities that are useful in isolation
                AddToPool(ability_pool, "PTTruesight", psitracker);
                AddToPool(ability_pool, "PTAlacrity", psitracker);
                AddToPool(ability_pool, "PTPrecision", psitracker);
                AddToPool(ability_pool, "PTSerenity", psitracker);
                AddToPool(ability_pool, "PTPsiTrance", psitracker);
                AddToPool(ability_pool, "PTPsiForging", psitracker);

                // Some abilities require basic capability checking
                if (!trainee.skills.GetSkill(SkillDefOf.Melee).TotallyDisabled)
                {
                    AddToPool(ability_pool, "PTCombatPrecognition", psitracker);
                    AddToPool(ability_pool, "PTCombatInsight", psitracker);
                }
                if (!trainee.skills.GetSkill(SkillDefOf.Shooting).TotallyDisabled)
                {
                    AddToPool(ability_pool, "PTCombatIntuition", psitracker);
                    AddToPool(ability_pool, "PTCombatPrecision", psitracker);
                }
                if (!trainee.skills.GetSkill(SkillDefOf.Social).TotallyDisabled)
                {
                    AddToPool(ability_pool, "PTEmpatheticNegotiation", psitracker);
                    AddToPool(ability_pool, "PTEmpatheticBargaining", psitracker);
                }
                // Oddly, Animals skill is not "TotallyDisabled" even if you de facto cannot use it.
                if (!trainee.WorkTagIsDisabled(WorkTags.Animals))
                {
                    AddToPool(ability_pool, "PTEmpatheticHandling", psitracker);
                }
                
                // Psi Attunement is bad if you have no other abilities
                if (psitracker.Abilities.Count != 0)
                {
                    AddToPool(ability_pool, "PTPsiAttunement", psitracker);
                }
                // Try very weak abilities as a last resort
                if (!ability_pool.Any())
                {
                    AddToPool(ability_pool, "PTOverdrive", psitracker);
                    AddToPool(ability_pool, "PTInsulation", psitracker);
                    AddToPool(ability_pool, "PTPurity", psitracker);
                    AddToPool(ability_pool, "PTConversion", psitracker);
                }
                // If ability pool is still empty do nothing
                if(ability_pool.Any())
                {
                    psitracker.AddAbility(ability_pool.RandomElement());
                }
            }
        }
    }
}
