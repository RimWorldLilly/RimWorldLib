using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F82 RID: 8066
	public class StatPart_EnvironmentalEffects : StatPart
	{
		// Token: 0x0600CD27 RID: 52519 RVA: 0x004B7244 File Offset: 0x004B5444
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num = 0f;
			if (!this.ActiveFor(req.Thing))
			{
				return;
			}
			Thing thing = req.Thing;
			if (!thing.Position.Roofed(thing.Map))
			{
				num += this.factorOffsetUnroofed;
			}
			Room room = thing.GetRoom(RegionType.Set_All);
			if (room != null && room.UsesOutdoorTemperature)
			{
				num += this.factorOffsetOutdoors;
			}
			TerrainDef terrain = thing.Position.GetTerrain(thing.Map);
			if (terrain != null && terrain.extraDeteriorationFactor != 0f)
			{
				num += terrain.extraDeteriorationFactor;
			}
			if (!thing.Position.Roofed(thing.Map))
			{
				num *= Mathf.Lerp(1f, 5f, thing.Map.weatherManager.RainRate);
			}
			if (SteadyEnvironmentEffects.ProtectedByEdifice(thing.Position, thing.Map))
			{
				num *= this.protectedByEdificeFactor;
			}
			val *= num;
		}

		// Token: 0x0600CD28 RID: 52520 RVA: 0x004B732C File Offset: 0x004B552C
		public override string ExplanationPart(StatRequest req)
		{
			if (!this.ActiveFor(req.Thing))
			{
				return null;
			}
			Thing thing = req.Thing;
			StringBuilder stringBuilder = new StringBuilder();
			if (!thing.Position.Roofed(thing.Map))
			{
				stringBuilder.AppendLine("DeterioratingUnroofed".Translate().CapitalizeFirst() + (": +" + this.factorOffsetUnroofed.ToStringPercent()));
			}
			Room room = thing.GetRoom(RegionType.Set_All);
			if (room != null && room.UsesOutdoorTemperature)
			{
				stringBuilder.AppendLine("DeterioratingOutdoors".Translate().CapitalizeFirst() + (": +" + this.factorOffsetOutdoors.ToStringPercent()));
			}
			TerrainDef terrain = thing.Position.GetTerrain(thing.Map);
			if (terrain != null && terrain.extraDeteriorationFactor != 0f)
			{
				stringBuilder.AppendLine(terrain.LabelCap + (": +" + terrain.extraDeteriorationFactor.ToStringPercent()));
			}
			if (!thing.Position.Roofed(thing.Map) && thing.Map.weatherManager.RainRate > 0f)
			{
				stringBuilder.AppendLine("DeterioratingRaining".Translate().CapitalizeFirst() + (": x" + Mathf.Lerp(1f, 5f, thing.Map.weatherManager.RainRate).ToStringPercent()));
			}
			if (SteadyEnvironmentEffects.ProtectedByEdifice(thing.Position, thing.Map))
			{
				stringBuilder.AppendLine("Protected".Translate().CapitalizeFirst() + (": x" + this.protectedByEdificeFactor.ToStringPercent()));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600CD29 RID: 52521 RVA: 0x004B750D File Offset: 0x004B570D
		private bool ActiveFor(Thing t)
		{
			return t != null && t.Spawned && t.def.deteriorateFromEnvironmentalEffects;
		}

		// Token: 0x040082F6 RID: 33526
		private float factorOffsetUnroofed;

		// Token: 0x040082F7 RID: 33527
		private float factorOffsetOutdoors;

		// Token: 0x040082F8 RID: 33528
		private float protectedByEdificeFactor;
	}
}
