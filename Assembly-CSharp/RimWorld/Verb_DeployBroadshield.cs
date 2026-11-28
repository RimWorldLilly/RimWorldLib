using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001FE3 RID: 8163
	public class Verb_DeployBroadshield : Verb
	{
		// Token: 0x0600CF05 RID: 52997 RVA: 0x004C42FD File Offset: 0x004C24FD
		protected override bool TryCastShot()
		{
			return Verb_DeployBroadshield.Deploy(base.ReloadableCompSource);
		}

		// Token: 0x0600CF06 RID: 52998 RVA: 0x004C430C File Offset: 0x004C250C
		public static bool Deploy(CompApparelReloadable comp)
		{
			if (!ModLister.CheckRoyalty("Projectile interceptors"))
			{
				return false;
			}
			string text;
			if (comp == null || !comp.CanBeUsed(out text))
			{
				return false;
			}
			Pawn wearer = comp.Wearer;
			Map map = wearer.Map;
			int num = GenRadial.NumCellsInRadius(4f);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = wearer.Position + GenRadial.RadialPattern[i];
				if (intVec.IsValid && intVec.InBounds(map))
				{
					Verb_DeployBroadshield.SpawnEffect(GenSpawn.Spawn(ThingDefOf.BroadshieldProjector, intVec, map, WipeMode.Vanish));
					comp.UsedOnce();
					return true;
				}
			}
			Messages.Message("AbilityNotEnoughFreeSpace".Translate(), wearer, MessageTypeDefOf.RejectInput, false);
			return false;
		}

		// Token: 0x0600CF07 RID: 52999 RVA: 0x004C43C7 File Offset: 0x004C25C7
		private static void SpawnEffect(Thing projector)
		{
			FleckMaker.Static(projector.TrueCenter(), projector.Map, FleckDefOf.BroadshieldActivation, 1f);
			SoundDefOf.Broadshield_Startup.PlayOneShot(new TargetInfo(projector.Position, projector.Map, false));
		}
	}
}
