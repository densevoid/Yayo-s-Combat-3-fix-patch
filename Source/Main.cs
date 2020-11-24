using HarmonyLib;
using RimWorld;
using System.Reflection;
using System.Text;
using Verse;

namespace yayoCombatFix
{
    [StaticConstructorOnStartup]
    class Main
    {
        static Main()
        {
            var harmony = new Harmony("io.github.densevoid.combat3fix");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Log.Message("yayo combat fixed!");
        }
    }


    [HarmonyPatch(typeof(StatPart_ReloadMarketValue), "TransformAndExplain")]
    public class Patch_StatPart_ReloadMarketValue_TransformAndExplain
    {
        public static bool Prefix(StatRequest req, ref float val, StringBuilder explanation)
        {
            if (req != null && req.Thing != null && req.Thing.def != null)

                if (req.Thing.def.IsRangedWeapon)
                {
                    CompReloadable compReloadable = req.Thing.TryGetComp<CompReloadable>();
                    if (compReloadable != null)
                    {
                        if (compReloadable.AmmoDef != null && compReloadable.RemainingCharges != 0)
                        {
                            int num = compReloadable.RemainingCharges;
                            float chargesPrice = compReloadable.AmmoDef.BaseMarketValue * (float)num;
                            val += chargesPrice;
                            explanation?.AppendLine("StatsReport_ReloadMarketValue".Translate(NamedArgumentUtility.Named(compReloadable.AmmoDef, "AMMO"), num.Named("COUNT")) + ": " + chargesPrice.ToStringMoneyOffset());
                        }

                        return false;
                    }
                }

            return true;
        }
    }
}
