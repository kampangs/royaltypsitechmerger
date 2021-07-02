using RimWorld;
using Verse;

namespace Royalty_PsiTech_Merger
{
    public class CompUseEffect_GainPsiTechAbility : CompUseEffect
    {
        public override void DoEffect(Pawn user)
        {
            BlindPsiTrainingUtility.Train(user);
        }
    }
}
