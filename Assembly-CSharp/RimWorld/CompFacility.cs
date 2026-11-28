using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200187A RID: 6266
	public class CompFacility : ThingComp
	{
		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06009CDF RID: 40159 RVA: 0x00387180 File Offset: 0x00385380
		// (remove) Token: 0x06009CE0 RID: 40160 RVA: 0x003871B8 File Offset: 0x003853B8
		public event Action<CompFacility, Thing> OnLinkAdded;

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x06009CE1 RID: 40161 RVA: 0x003871F0 File Offset: 0x003853F0
		// (remove) Token: 0x06009CE2 RID: 40162 RVA: 0x00387228 File Offset: 0x00385428
		public event Action<CompFacility, Thing> OnLinkRemoved;

		// Token: 0x17001AD6 RID: 6870
		// (get) Token: 0x06009CE3 RID: 40163 RVA: 0x00387260 File Offset: 0x00385460
		public virtual bool CanBeActive
		{
			get
			{
				CompPowerTrader compPowerTrader = this.parent.TryGetComp<CompPowerTrader>();
				return compPowerTrader == null || compPowerTrader.PowerOn;
			}
		}

		// Token: 0x17001AD7 RID: 6871
		// (get) Token: 0x06009CE4 RID: 40164 RVA: 0x00387287 File Offset: 0x00385487
		public List<Thing> LinkedBuildings
		{
			get
			{
				return this.linkedBuildings;
			}
		}

		// Token: 0x17001AD8 RID: 6872
		// (get) Token: 0x06009CE5 RID: 40165 RVA: 0x0038728F File Offset: 0x0038548F
		public virtual List<StatModifier> StatOffsets
		{
			get
			{
				return this.Props.statOffsets;
			}
		}

		// Token: 0x17001AD9 RID: 6873
		// (get) Token: 0x06009CE6 RID: 40166 RVA: 0x0038729C File Offset: 0x0038549C
		public CompProperties_Facility Props
		{
			get
			{
				return (CompProperties_Facility)this.props;
			}
		}

		// Token: 0x17001ADA RID: 6874
		// (get) Token: 0x06009CE7 RID: 40167 RVA: 0x003872A9 File Offset: 0x003854A9
		protected virtual string MaxConnectedString
		{
			get
			{
				return "FacilityMaxSimultaneousConnections".Translate();
			}
		}

		// Token: 0x06009CE8 RID: 40168 RVA: 0x003872BC File Offset: 0x003854BC
		public static void DrawLinesToPotentialThingsToLinkTo(ThingDef myDef, IntVec3 myPos, Rot4 myRot, Map map)
		{
			CompProperties_Facility compProperties = myDef.GetCompProperties<CompProperties_Facility>();
			if (((compProperties != null) ? compProperties.linkableBuildings : null) == null)
			{
				return;
			}
			Vector3 a = GenThing.TrueCenter(myPos, myRot, myDef.size, myDef.Altitude);
			for (int i = 0; i < compProperties.linkableBuildings.Count; i++)
			{
				foreach (Thing thing in map.listerThings.ThingsOfDef(compProperties.linkableBuildings[i]))
				{
					CompAffectedByFacilities compAffectedByFacilities = thing.TryGetComp<CompAffectedByFacilities>();
					if (compAffectedByFacilities != null && compAffectedByFacilities.CanPotentiallyLinkTo(myDef, myPos, myRot))
					{
						GenDraw.DrawLineBetween(a, thing.TrueCenter());
						compAffectedByFacilities.DrawRedLineToPotentiallySupplantedFacility(myDef, myPos, myRot);
					}
				}
			}
		}

		// Token: 0x06009CE9 RID: 40169 RVA: 0x00387390 File Offset: 0x00385590
		public static void DrawPlaceMouseAttachmentsToPotentialThingsToLinkTo(float curX, ref float curY, ThingDef myDef, IntVec3 myPos, Rot4 myRot, Map map)
		{
			CompFacility.<>c__DisplayClass19_0 CS$<>8__locals1;
			CS$<>8__locals1.curX = curX;
			CompProperties_Facility compProperties = myDef.GetCompProperties<CompProperties_Facility>();
			int num = 0;
			for (int i = 0; i < compProperties.linkableBuildings.Count; i++)
			{
				foreach (Thing thing in map.listerThings.ThingsOfDef(compProperties.linkableBuildings[i]))
				{
					CompAffectedByFacilities compAffectedByFacilities = thing.TryGetComp<CompAffectedByFacilities>();
					if (compAffectedByFacilities != null && compAffectedByFacilities.CanPotentiallyLinkTo(myDef, myPos, myRot))
					{
						num++;
						if (num == 1)
						{
							CompFacility.<DrawPlaceMouseAttachmentsToPotentialThingsToLinkTo>g__DrawTextLine|19_0(ref curY, "FacilityPotentiallyLinkedTo".Translate() + ":", ref CS$<>8__locals1);
						}
						CompFacility.<DrawPlaceMouseAttachmentsToPotentialThingsToLinkTo>g__DrawTextLine|19_0(ref curY, "  - " + thing.LabelCap, ref CS$<>8__locals1);
					}
				}
			}
			if (num == 0)
			{
				CompFacility.<DrawPlaceMouseAttachmentsToPotentialThingsToLinkTo>g__DrawTextLine|19_0(ref curY, "FacilityNoPotentialLinks".Translate(), ref CS$<>8__locals1);
			}
		}

		// Token: 0x06009CEA RID: 40170 RVA: 0x00387494 File Offset: 0x00385694
		public override void CompTick()
		{
			base.CompTick();
			if (this.Props.mustBePlacedFacingThingLinear && this.parent.Spawned && this.parent.IsHashIntervalTick(120))
			{
				bool flag = ContainmentUtility.IsLinearBuildingBlocked(this.parent.def, this.parent.Position, this.parent.Rotation, this.parent.Map, null);
				if ((this.linkedBuildings.Any<Thing>() && flag) || (this.linkedBuildings.Empty<Thing>() && !flag))
				{
					this.RelinkAll();
				}
			}
		}

		// Token: 0x06009CEB RID: 40171 RVA: 0x00387528 File Offset: 0x00385728
		public virtual bool CanLink()
		{
			return true;
		}

		// Token: 0x06009CEC RID: 40172 RVA: 0x0038752C File Offset: 0x0038572C
		public void Notify_NewLink(Thing thing)
		{
			for (int i = 0; i < this.linkedBuildings.Count; i++)
			{
				if (this.linkedBuildings[i] == thing)
				{
					Log.Error("Notify_NewLink was called but the link is already here.");
					return;
				}
			}
			this.linkedBuildings.Add(thing);
			Action<CompFacility, Thing> onLinkAdded = this.OnLinkAdded;
			if (onLinkAdded == null)
			{
				return;
			}
			onLinkAdded(this, thing);
		}

		// Token: 0x06009CED RID: 40173 RVA: 0x00387588 File Offset: 0x00385788
		public void Notify_LinkRemoved(Thing thing)
		{
			int i = 0;
			while (i < this.linkedBuildings.Count)
			{
				if (this.linkedBuildings[i] == thing)
				{
					this.linkedBuildings.RemoveAt(i);
					Action<CompFacility, Thing> onLinkRemoved = this.OnLinkRemoved;
					if (onLinkRemoved == null)
					{
						return;
					}
					onLinkRemoved(this, thing);
					return;
				}
				else
				{
					i++;
				}
			}
			Log.Error("Notify_LinkRemoved was called but there is no such link here.");
		}

		// Token: 0x06009CEE RID: 40174 RVA: 0x003875E3 File Offset: 0x003857E3
		public void Notify_LOSBlockerSpawnedOrDespawned()
		{
			this.RelinkAll();
		}

		// Token: 0x06009CEF RID: 40175 RVA: 0x003875EB File Offset: 0x003857EB
		public void Notify_ThingChanged()
		{
			this.RelinkAll();
		}

		// Token: 0x06009CF0 RID: 40176 RVA: 0x003875F3 File Offset: 0x003857F3
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.LinkToNearbyBuildings();
		}

		// Token: 0x06009CF1 RID: 40177 RVA: 0x003875FB File Offset: 0x003857FB
		public override void PostMapInit()
		{
			this.RelinkAll();
		}

		// Token: 0x06009CF2 RID: 40178 RVA: 0x00387604 File Offset: 0x00385804
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			this.thingsToNotify.Clear();
			for (int i = 0; i < this.linkedBuildings.Count; i++)
			{
				this.thingsToNotify.Add(this.linkedBuildings[i]);
			}
			this.UnlinkAll();
			foreach (Thing thing in this.thingsToNotify)
			{
				thing.TryGetComp<CompAffectedByFacilities>().Notify_FacilityDespawned();
			}
		}

		// Token: 0x06009CF3 RID: 40179 RVA: 0x00387698 File Offset: 0x00385898
		public override void PostDrawExtraSelectionOverlays()
		{
			for (int i = 0; i < this.linkedBuildings.Count; i++)
			{
				if (this.linkedBuildings[i].TryGetComp<CompAffectedByFacilities>().IsFacilityActive(this.parent))
				{
					GenDraw.DrawLineBetween(this.parent.TrueCenter(), this.linkedBuildings[i].TrueCenter());
				}
				else
				{
					GenDraw.DrawLineBetween(this.parent.TrueCenter(), this.linkedBuildings[i].TrueCenter(), CompAffectedByFacilities.InactiveFacilityLineMat, 0.2f);
				}
			}
		}

		// Token: 0x06009CF4 RID: 40180 RVA: 0x00387728 File Offset: 0x00385928
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.StatOffsets != null)
			{
				bool flag = this.AmIActiveForAnyone();
				for (int i = 0; i < this.StatOffsets.Count; i++)
				{
					StatDef stat = this.StatOffsets[i].stat;
					stringBuilder.Append(stat.OffsetLabelCap);
					stringBuilder.Append(": ");
					stringBuilder.Append(this.StatOffsets[i].ValueToStringAsOffset);
					if (!flag)
					{
						stringBuilder.Append(" (");
						stringBuilder.Append("InactiveFacility".Translate());
						stringBuilder.Append(")");
					}
					if (i < this.StatOffsets.Count - 1)
					{
						stringBuilder.AppendLine();
					}
				}
				stringBuilder.Append("\n");
			}
			CompProperties_Facility props = this.Props;
			if (props.showMaxSimultaneous)
			{
				stringBuilder.Append(this.MaxConnectedString);
				stringBuilder.Append(": " + props.maxSimultaneous.ToString());
			}
			if (props.mustBePlacedFacingThingLinear && this.parent.Spawned && ContainmentUtility.IsLinearBuildingBlocked(this.parent.def, this.parent.Position, this.parent.Rotation, this.parent.Map, null))
			{
				stringBuilder.AppendInNewLine("FacilityFrontBlocked".Translate());
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x06009CF5 RID: 40181 RVA: 0x003878A4 File Offset: 0x00385AA4
		private void RelinkAll()
		{
			this.LinkToNearbyBuildings();
		}

		// Token: 0x06009CF6 RID: 40182 RVA: 0x003878AC File Offset: 0x00385AAC
		private void LinkToNearbyBuildings()
		{
			this.UnlinkAll();
			CompProperties_Facility props = this.Props;
			if (props.linkableBuildings == null)
			{
				return;
			}
			for (int i = 0; i < props.linkableBuildings.Count; i++)
			{
				foreach (Thing thing in this.parent.Map.listerThings.ThingsOfDef(props.linkableBuildings[i]))
				{
					CompAffectedByFacilities compAffectedByFacilities = thing.TryGetComp<CompAffectedByFacilities>();
					if (compAffectedByFacilities != null && compAffectedByFacilities.CanLinkTo(this.parent))
					{
						this.linkedBuildings.Add(thing);
						compAffectedByFacilities.Notify_NewLink(this.parent);
						Action<CompFacility, Thing> onLinkAdded = this.OnLinkAdded;
						if (onLinkAdded != null)
						{
							onLinkAdded(this, compAffectedByFacilities.parent);
						}
					}
				}
			}
		}

		// Token: 0x06009CF7 RID: 40183 RVA: 0x00387994 File Offset: 0x00385B94
		private bool AmIActiveForAnyone()
		{
			for (int i = 0; i < this.linkedBuildings.Count; i++)
			{
				if (this.linkedBuildings[i].TryGetComp<CompAffectedByFacilities>().IsFacilityActive(this.parent))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009CF8 RID: 40184 RVA: 0x003879D8 File Offset: 0x00385BD8
		private void UnlinkAll()
		{
			for (int i = 0; i < this.linkedBuildings.Count; i++)
			{
				this.linkedBuildings[i].TryGetComp<CompAffectedByFacilities>().Notify_LinkRemoved(this.parent);
				Action<CompFacility, Thing> onLinkRemoved = this.OnLinkRemoved;
				if (onLinkRemoved != null)
				{
					onLinkRemoved(this, this.linkedBuildings[i]);
				}
			}
			this.linkedBuildings.Clear();
		}

		// Token: 0x06009CFA RID: 40186 RVA: 0x00387A60 File Offset: 0x00385C60
		[CompilerGenerated]
		internal static void <DrawPlaceMouseAttachmentsToPotentialThingsToLinkTo>g__DrawTextLine|19_0(ref float y, string text, ref CompFacility.<>c__DisplayClass19_0 A_2)
		{
			float lineHeight = Text.LineHeight;
			Widgets.Label(new Rect(A_2.curX, y, 999f, lineHeight), text);
			y += lineHeight;
		}

		// Token: 0x04005D0C RID: 23820
		private List<Thing> linkedBuildings = new List<Thing>();

		// Token: 0x04005D0D RID: 23821
		private const int UpdateRateIntervalTicks = 120;

		// Token: 0x04005D10 RID: 23824
		private HashSet<Thing> thingsToNotify = new HashSet<Thing>();
	}
}
