using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AE5 RID: 6885
	public class Blight : Thing
	{
		// Token: 0x17001FB2 RID: 8114
		// (get) Token: 0x0600AFD1 RID: 45009 RVA: 0x003DF8AA File Offset: 0x003DDAAA
		// (set) Token: 0x0600AFD2 RID: 45010 RVA: 0x003DF8B2 File Offset: 0x003DDAB2
		public float Severity
		{
			get
			{
				return this.severity;
			}
			set
			{
				this.severity = Mathf.Clamp01(value);
			}
		}

		// Token: 0x17001FB3 RID: 8115
		// (get) Token: 0x0600AFD3 RID: 45011 RVA: 0x003DF8C0 File Offset: 0x003DDAC0
		public Plant Plant
		{
			get
			{
				if (!base.Spawned)
				{
					return null;
				}
				return BlightUtility.GetFirstBlightableEverPlant(base.Position, base.Map);
			}
		}

		// Token: 0x17001FB4 RID: 8116
		// (get) Token: 0x0600AFD4 RID: 45012 RVA: 0x003DF8DD File Offset: 0x003DDADD
		protected float ReproduceMTBHours
		{
			get
			{
				if (this.severity < 0.28f)
				{
					return -1f;
				}
				return GenMath.LerpDouble(0.28f, 1f, 16.8f, 2.1f, this.severity);
			}
		}

		// Token: 0x0600AFD5 RID: 45013 RVA: 0x003DF911 File Offset: 0x003DDB11
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.severity, "severity", 0f, false);
			Scribe_Values.Look<int>(ref this.lastPlantHarmTick, "lastPlantHarmTick", 0, false);
		}

		// Token: 0x0600AFD6 RID: 45014 RVA: 0x003DF941 File Offset: 0x003DDB41
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad && !base.BeingTransportedOnGravship)
			{
				this.lastPlantHarmTick = Find.TickManager.TicksGame;
			}
			this.lastMapMeshUpdateSeverity = this.Severity;
		}

		// Token: 0x0600AFD7 RID: 45015 RVA: 0x003DF974 File Offset: 0x003DDB74
		public override void TickLong()
		{
			this.CheckHarmPlant();
			if (this.DestroyIfNoPlantHere())
			{
				return;
			}
			this.Severity += 0.033333335f;
			float reproduceMTBHours = this.ReproduceMTBHours;
			if (reproduceMTBHours > 0f && Rand.MTBEventOccurs(reproduceMTBHours, 2500f, 2000f))
			{
				this.TryReproduceNow();
			}
			if (Mathf.Abs(this.Severity - this.lastMapMeshUpdateSeverity) >= 0.05f)
			{
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlagDefOf.Things);
				this.lastMapMeshUpdateSeverity = this.Severity;
			}
		}

		// Token: 0x0600AFD8 RID: 45016 RVA: 0x003DFA0E File Offset: 0x003DDC0E
		public void Notify_PlantDeSpawned()
		{
			this.DestroyIfNoPlantHere();
		}

		// Token: 0x0600AFD9 RID: 45017 RVA: 0x003DFA17 File Offset: 0x003DDC17
		private bool DestroyIfNoPlantHere()
		{
			if (base.Destroyed)
			{
				return true;
			}
			if (this.Plant == null)
			{
				this.Destroy(DestroyMode.Vanish);
				return true;
			}
			return false;
		}

		// Token: 0x0600AFDA RID: 45018 RVA: 0x003DFA38 File Offset: 0x003DDC38
		private void CheckHarmPlant()
		{
			int ticksGame = Find.TickManager.TicksGame;
			if (ticksGame - this.lastPlantHarmTick >= 60000)
			{
				List<Thing> thingList = base.Position.GetThingList(base.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Plant plant = thingList[i] as Plant;
					if (plant != null)
					{
						this.HarmPlant(plant);
					}
				}
				this.lastPlantHarmTick = ticksGame;
			}
		}

		// Token: 0x0600AFDB RID: 45019 RVA: 0x003DFAA0 File Offset: 0x003DDCA0
		private void HarmPlant(Plant plant)
		{
			bool isCrop = plant.IsCrop;
			IntVec3 position = base.Position;
			Map map = base.Map;
			plant.TakeDamage(new DamageInfo(DamageDefOf.Rotting, 5f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
			if (plant.Destroyed && isCrop && MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfBlight-" + plant.def.defName, 240f))
			{
				Messages.Message("MessagePlantDiedOfBlight".Translate(plant.Label, plant).CapitalizeFirst(), new TargetInfo(position, map, false), MessageTypeDefOf.NegativeEvent, true);
			}
		}

		// Token: 0x0600AFDC RID: 45020 RVA: 0x003DFB57 File Offset: 0x003DDD57
		public void TryReproduceNow()
		{
			GenRadial.ProcessEquidistantCells(base.Position, 4f, delegate(List<IntVec3> cells)
			{
				IntVec3 c;
				if ((from x in cells
				where BlightUtility.GetFirstBlightableNowPlant(x, base.Map) != null
				select x).TryRandomElement(out c))
				{
					BlightUtility.GetFirstBlightableNowPlant(c, base.Map).CropBlighted();
					return true;
				}
				return false;
			}, base.Map);
		}

		// Token: 0x0600AFDD RID: 45021 RVA: 0x003DFB7C File Offset: 0x003DDD7C
		public override void Print(SectionLayer layer)
		{
			Plant plant = this.Plant;
			if (plant != null)
			{
				PlantUtility.SetWindExposureColors(Blight.workingColors, plant);
			}
			else
			{
				Blight.workingColors[0].a = (Blight.workingColors[1].a = (Blight.workingColors[2].a = (Blight.workingColors[3].a = 0)));
			}
			float num = Blight.SizeRange.LerpThroughRange(this.severity);
			if (plant != null)
			{
				float a = plant.Graphic.drawSize.x * plant.def.plant.visualSizeRange.LerpThroughRange(plant.Growth);
				num *= Mathf.Min(a, 1f);
			}
			num = Mathf.Clamp(num, 0.5f, 0.9f);
			Printer_Plane.PrintPlane(layer, this.TrueCenter(), this.def.graphic.drawSize * num, this.Graphic.MatAt(base.Rotation, this), 0f, false, null, Blight.workingColors, 0.1f, 0f);
		}

		// Token: 0x04006579 RID: 25977
		private float severity = 0.2f;

		// Token: 0x0400657A RID: 25978
		private int lastPlantHarmTick;

		// Token: 0x0400657B RID: 25979
		private float lastMapMeshUpdateSeverity;

		// Token: 0x0400657C RID: 25980
		private const float InitialSeverity = 0.2f;

		// Token: 0x0400657D RID: 25981
		private const float SeverityPerDay = 1f;

		// Token: 0x0400657E RID: 25982
		private const int DamagePerDay = 5;

		// Token: 0x0400657F RID: 25983
		private const float MinSeverityToReproduce = 0.28f;

		// Token: 0x04006580 RID: 25984
		private const float ReproduceMTBHoursAtMinSeverity = 16.8f;

		// Token: 0x04006581 RID: 25985
		private const float ReproduceMTBHoursAtMaxSeverity = 2.1f;

		// Token: 0x04006582 RID: 25986
		private const float ReproductionRadius = 4f;

		// Token: 0x04006583 RID: 25987
		private static FloatRange SizeRange = new FloatRange(0.6f, 1f);

		// Token: 0x04006584 RID: 25988
		private static Color32[] workingColors = new Color32[4];
	}
}
