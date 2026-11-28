using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F46 RID: 8006
	public static class ShortCircuitUtility
	{
		// Token: 0x0600CC58 RID: 52312 RVA: 0x004B5021 File Offset: 0x004B3221
		public static IEnumerable<Building> GetShortCircuitablePowerConduits(Map map)
		{
			ShortCircuitUtility.<GetShortCircuitablePowerConduits>d__1 <GetShortCircuitablePowerConduits>d__ = new ShortCircuitUtility.<GetShortCircuitablePowerConduits>d__1(-2);
			<GetShortCircuitablePowerConduits>d__.<>3__map = map;
			return <GetShortCircuitablePowerConduits>d__;
		}

		// Token: 0x0600CC59 RID: 52313 RVA: 0x004B5034 File Offset: 0x004B3234
		public static void DoShortCircuit(Building culprit)
		{
			PowerNet powerNet = culprit.PowerComp.PowerNet;
			Map map = culprit.Map;
			float num = 0f;
			float num2 = 0f;
			bool flag = false;
			if (powerNet.batteryComps.Any((CompPowerBattery x) => x.StoredEnergy > 20f))
			{
				ShortCircuitUtility.DrainBatteriesAndCauseExplosion(powerNet, culprit, out num, out num2);
			}
			else
			{
				flag = ShortCircuitUtility.TryStartFireNear(culprit);
			}
			string value;
			if (culprit.def == ThingDefOf.PowerConduit)
			{
				value = "AnElectricalConduit".Translate();
			}
			else
			{
				value = Find.ActiveLanguageWorker.WithIndefiniteArticlePostProcessed(culprit.Label, false, false);
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (flag)
			{
				stringBuilder.Append("ShortCircuitStartedFire".Translate(value));
			}
			else
			{
				stringBuilder.Append("ShortCircuit".Translate(value));
			}
			if (num > 0f)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				stringBuilder.Append("ShortCircuitDischargedEnergy".Translate(num.ToString("F0")));
			}
			if (num2 > 5f)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				stringBuilder.Append("ShortCircuitWasLarge".Translate());
			}
			if (num2 > 8f)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				stringBuilder.Append("ShortCircuitWasHuge".Translate());
			}
			Find.LetterStack.ReceiveLetter("LetterLabelShortCircuit".Translate(), stringBuilder.ToString(), LetterDefOf.NegativeEvent, new TargetInfo(culprit.Position, map, false), null, null, null, null, 0, true);
		}

		// Token: 0x0600CC5A RID: 52314 RVA: 0x004B51FC File Offset: 0x004B33FC
		public static bool TryShortCircuitInRain(Thing thing)
		{
			CompPowerTrader compPowerTrader = thing.TryGetComp<CompPowerTrader>();
			if ((compPowerTrader != null && compPowerTrader.PowerOn && compPowerTrader.Props.shortCircuitInRain) || (thing.TryGetComp<CompPowerBattery>() != null && thing.TryGetComp<CompPowerBattery>().StoredEnergy > 100f))
			{
				TaggedString taggedString = "ShortCircuitRain".Translate(thing.Label, thing);
				TargetInfo target = new TargetInfo(thing.Position, thing.Map, false);
				if (thing.Faction == Faction.OfPlayer)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelShortCircuit".Translate(), taggedString, LetterDefOf.NegativeEvent, target, null, null, null, null, 0, true);
				}
				else
				{
					Messages.Message(taggedString, target, MessageTypeDefOf.NeutralEvent, true);
				}
				GenExplosion.DoExplosion(thing.OccupiedRect().RandomCell, thing.Map, 1.9f, DamageDefOf.Flame, null, -1, -1f, null, null, null, null, null, 0f, 1, null, null, 255, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
				return true;
			}
			return false;
		}

		// Token: 0x0600CC5B RID: 52315 RVA: 0x004B5350 File Offset: 0x004B3550
		private static void DrainBatteriesAndCauseExplosion(PowerNet net, Building culprit, out float totalEnergy, out float explosionRadius)
		{
			totalEnergy = 0f;
			for (int i = 0; i < net.batteryComps.Count; i++)
			{
				CompPowerBattery compPowerBattery = net.batteryComps[i];
				totalEnergy += compPowerBattery.StoredEnergy;
				compPowerBattery.DrawPower(compPowerBattery.StoredEnergy);
			}
			explosionRadius = Mathf.Sqrt(totalEnergy) * 0.05f;
			explosionRadius = Mathf.Clamp(explosionRadius, 1.5f, 14.9f);
			GenExplosion.DoExplosion(culprit.Position, net.Map, explosionRadius, DamageDefOf.Flame, null, -1, -1f, null, null, null, null, null, 0f, 1, null, null, 255, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
			if (explosionRadius > 3.5f)
			{
				GenExplosion.DoExplosion(culprit.Position, net.Map, explosionRadius * 0.3f, DamageDefOf.Bomb, null, -1, -1f, null, null, null, null, null, 0f, 1, null, null, 255, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
			}
		}

		// Token: 0x0600CC5C RID: 52316 RVA: 0x004B54C4 File Offset: 0x004B36C4
		private static bool TryStartFireNear(Building b)
		{
			ShortCircuitUtility.tmpCells.Clear();
			int num = GenRadial.NumCellsInRadius(3f);
			CellRect startRect = b.OccupiedRect();
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = b.Position + GenRadial.RadialPattern[i];
				if (GenSight.LineOfSight(b.Position, intVec, b.Map, startRect, CellRect.SingleCell(intVec), null, false) && FireUtility.ChanceToStartFireIn(intVec, b.Map, null) > 0f)
				{
					ShortCircuitUtility.tmpCells.Add(intVec);
				}
			}
			return ShortCircuitUtility.tmpCells.Any<IntVec3>() && FireUtility.TryStartFireIn(ShortCircuitUtility.tmpCells.RandomElement<IntVec3>(), b.Map, Rand.Range(0.1f, 1.75f), null, null);
		}

		// Token: 0x04008212 RID: 33298
		private static Dictionary<PowerNet, bool> tmpPowerNetHasActivePowerSource = new Dictionary<PowerNet, bool>();

		// Token: 0x04008213 RID: 33299
		private static List<IntVec3> tmpCells = new List<IntVec3>();
	}
}
