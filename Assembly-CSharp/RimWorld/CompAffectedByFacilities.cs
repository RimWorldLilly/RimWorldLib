using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001800 RID: 6144
	[StaticConstructorOnStartup]
	public class CompAffectedByFacilities : ThingComp
	{
		// Token: 0x17001A11 RID: 6673
		// (get) Token: 0x06009960 RID: 39264 RVA: 0x00377AD8 File Offset: 0x00375CD8
		public List<Thing> LinkedFacilitiesListForReading
		{
			get
			{
				return this.linkedFacilities;
			}
		}

		// Token: 0x06009961 RID: 39265 RVA: 0x00377AE0 File Offset: 0x00375CE0
		public bool CanLinkTo(Thing facility)
		{
			CompFacility compFacility;
			if (!facility.TryGetComp(out compFacility))
			{
				return false;
			}
			if (!compFacility.CanLink())
			{
				return false;
			}
			if (!this.CanPotentiallyLinkTo(facility.def, facility.Position, facility.Rotation))
			{
				return false;
			}
			if (!this.IsValidFacilityForMe(facility))
			{
				return false;
			}
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				if (this.linkedFacilities[i] == facility)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06009962 RID: 39266 RVA: 0x00377B52 File Offset: 0x00375D52
		public static bool CanPotentiallyLinkTo_Static(Thing facility, ThingDef myDef, IntVec3 myPos, Rot4 myRot, Map myMap)
		{
			return CompAffectedByFacilities.CanPotentiallyLinkTo_Static(facility.def, facility.Position, facility.Rotation, myDef, myPos, myRot, myMap) && CompAffectedByFacilities.IsPotentiallyValidFacilityForMe_Static(facility, myDef, myPos, myRot);
		}

		// Token: 0x06009963 RID: 39267 RVA: 0x00377B84 File Offset: 0x00375D84
		public bool CanPotentiallyLinkTo(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot)
		{
			if (!CompAffectedByFacilities.CanPotentiallyLinkTo_Static(facilityDef, facilityPos, facilityRot, this.parent.def, this.parent.Position, this.parent.Rotation, this.parent.Map))
			{
				return false;
			}
			if (!this.IsPotentiallyValidFacilityForMe(facilityDef, facilityPos, facilityRot))
			{
				return false;
			}
			int num = 0;
			bool flag = false;
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				if (this.linkedFacilities[i].def == facilityDef)
				{
					num++;
					if (this.IsBetter(facilityDef, facilityPos, facilityRot, this.linkedFacilities[i]))
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				return true;
			}
			CompProperties_Facility compProperties = facilityDef.GetCompProperties<CompProperties_Facility>();
			return num + 1 <= compProperties.maxSimultaneous;
		}

		// Token: 0x06009964 RID: 39268 RVA: 0x00377C40 File Offset: 0x00375E40
		public static bool CanPotentiallyLinkTo_Static(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot, ThingDef myDef, IntVec3 myPos, Rot4 myRot, Map myMap)
		{
			CompProperties_Facility compProperties = facilityDef.GetCompProperties<CompProperties_Facility>();
			if (compProperties.mustBePlacedAdjacent)
			{
				CellRect rect = GenAdj.OccupiedRect(myPos, myRot, myDef.size);
				CellRect rect2 = GenAdj.OccupiedRect(facilityPos, facilityRot, facilityDef.size);
				if (!GenAdj.AdjacentTo8WayOrInside(rect, rect2))
				{
					return false;
				}
			}
			if (!compProperties.mustBePlacedFacingThingLinear)
			{
				if (compProperties.mustBePlacedAdjacentCardinalToBedHead || compProperties.mustBePlacedAdjacentCardinalToAndFacingBedHead)
				{
					if (!myDef.IsBed)
					{
						return false;
					}
					CellRect other = GenAdj.OccupiedRect(facilityPos, facilityRot, facilityDef.size);
					bool flag = false;
					int sleepingSlotsCount = BedUtility.GetSleepingSlotsCount(myDef.size);
					for (int i = 0; i < sleepingSlotsCount; i++)
					{
						IntVec3 sleepingSlotPos = BedUtility.GetSleepingSlotPos(i, myPos, myRot, myDef.size);
						if (sleepingSlotPos.IsAdjacentToCardinalOrInside(other))
						{
							if (compProperties.mustBePlacedAdjacentCardinalToAndFacingBedHead)
							{
								if (other.MovedBy(facilityRot.FacingCell).Contains(sleepingSlotPos))
								{
									flag = true;
								}
							}
							else
							{
								flag = true;
							}
						}
					}
					if (!flag)
					{
						return false;
					}
				}
				if (!compProperties.mustBePlacedAdjacent && !compProperties.mustBePlacedAdjacentCardinalToBedHead && !compProperties.mustBePlacedAdjacentCardinalToAndFacingBedHead)
				{
					Vector3 a = GenThing.TrueCenter(myPos, myRot, myDef.size, myDef.Altitude);
					Vector3 b = GenThing.TrueCenter(facilityPos, facilityRot, facilityDef.size, facilityDef.Altitude);
					float num = Vector3.Distance(a, b);
					if (num > compProperties.maxDistance || (compProperties.minDistance > 0f && num < compProperties.minDistance))
					{
						return false;
					}
				}
				return true;
			}
			if (ContainmentUtility.IsLinearBuildingBlocked(facilityDef, facilityPos, facilityRot, myMap, null))
			{
				return false;
			}
			CellRect cellRect = GenAdj.OccupiedRect(myPos, myRot, myDef.size);
			foreach (IntVec3 value in ContainmentUtility.GetInhibitorAffectedCells(facilityDef, facilityPos, facilityRot, myMap))
			{
				if (cellRect.Cells.Contains(value))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009965 RID: 39269 RVA: 0x00377E10 File Offset: 0x00376010
		public bool IsValidFacilityForMe(Thing facility)
		{
			return CompAffectedByFacilities.IsPotentiallyValidFacilityForMe_Static(facility, this.parent.def, this.parent.Position, this.parent.Rotation);
		}

		// Token: 0x06009966 RID: 39270 RVA: 0x00377E40 File Offset: 0x00376040
		private bool IsPotentiallyValidFacilityForMe(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot)
		{
			if (!CompAffectedByFacilities.IsPotentiallyValidFacilityForMe_Static(facilityDef, facilityPos, facilityRot, this.parent.def, this.parent.Position, this.parent.Rotation, this.parent.Map))
			{
				return false;
			}
			if (facilityDef.GetCompProperties<CompProperties_Facility>().canLinkToMedBedsOnly)
			{
				Building_Bed building_Bed = this.parent as Building_Bed;
				if (building_Bed == null || !building_Bed.Medical)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06009967 RID: 39271 RVA: 0x00377EAC File Offset: 0x003760AC
		private static bool IsPotentiallyValidFacilityForMe_Static(Thing facility, ThingDef myDef, IntVec3 myPos, Rot4 myRot)
		{
			return CompAffectedByFacilities.IsPotentiallyValidFacilityForMe_Static(facility.def, facility.Position, facility.Rotation, myDef, myPos, myRot, facility.Map);
		}

		// Token: 0x06009968 RID: 39272 RVA: 0x00377ED0 File Offset: 0x003760D0
		private static bool IsPotentiallyValidFacilityForMe_Static(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot, ThingDef myDef, IntVec3 myPos, Rot4 myRot, Map map)
		{
			if (!facilityDef.GetCompProperties<CompProperties_Facility>().requiresLOS)
			{
				return true;
			}
			CellRect cellRect = GenAdj.OccupiedRect(myPos, myRot, myDef.size);
			CellRect cellRect2 = GenAdj.OccupiedRect(facilityPos, facilityRot, facilityDef.size);
			bool result = false;
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					for (int k = cellRect2.minZ; k <= cellRect2.maxZ; k++)
					{
						for (int l = cellRect2.minX; l <= cellRect2.maxX; l++)
						{
							IntVec3 start = new IntVec3(j, 0, i);
							IntVec3 end = new IntVec3(l, 0, k);
							if (GenSight.LineOfSight(start, end, map, cellRect, cellRect2, null, false))
							{
								return true;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06009969 RID: 39273 RVA: 0x00377FA4 File Offset: 0x003761A4
		public void Notify_NewLink(Thing facility)
		{
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				if (this.linkedFacilities[i] == facility)
				{
					Log.Error("Notify_NewLink was called but the link is already here.");
					return;
				}
			}
			Thing potentiallySupplantedFacility = this.GetPotentiallySupplantedFacility(facility.def, facility.Position, facility.Rotation);
			if (potentiallySupplantedFacility != null)
			{
				potentiallySupplantedFacility.TryGetComp<CompFacility>().Notify_LinkRemoved(this.parent);
				this.linkedFacilities.Remove(potentiallySupplantedFacility);
			}
			this.linkedFacilities.Add(facility);
		}

		// Token: 0x0600996A RID: 39274 RVA: 0x00378028 File Offset: 0x00376228
		public void Notify_LinkRemoved(Thing thing)
		{
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				if (this.linkedFacilities[i] == thing)
				{
					this.linkedFacilities.RemoveAt(i);
					return;
				}
			}
			Log.Error("Notify_LinkRemoved was called but there is no such link here.");
		}

		// Token: 0x0600996B RID: 39275 RVA: 0x00378071 File Offset: 0x00376271
		public void Notify_FacilityDespawned()
		{
			this.RelinkAll();
		}

		// Token: 0x0600996C RID: 39276 RVA: 0x00378079 File Offset: 0x00376279
		public void Notify_LOSBlockerSpawnedOrDespawned()
		{
			this.RelinkAll();
		}

		// Token: 0x0600996D RID: 39277 RVA: 0x00378081 File Offset: 0x00376281
		public void Notify_ThingChanged()
		{
			this.RelinkAll();
		}

		// Token: 0x0600996E RID: 39278 RVA: 0x00378089 File Offset: 0x00376289
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.LinkToNearbyFacilities();
		}

		// Token: 0x0600996F RID: 39279 RVA: 0x00378091 File Offset: 0x00376291
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			this.UnlinkAll();
		}

		// Token: 0x06009970 RID: 39280 RVA: 0x0037809C File Offset: 0x0037629C
		public override void PostDrawExtraSelectionOverlays()
		{
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				if (this.IsFacilityActive(this.linkedFacilities[i]))
				{
					GenDraw.DrawLineBetween(this.parent.TrueCenter(), this.linkedFacilities[i].TrueCenter());
				}
				else
				{
					GenDraw.DrawLineBetween(this.parent.TrueCenter(), this.linkedFacilities[i].TrueCenter(), CompAffectedByFacilities.InactiveFacilityLineMat, 0.2f);
				}
			}
		}

		// Token: 0x06009971 RID: 39281 RVA: 0x00378124 File Offset: 0x00376324
		private bool IsBetter(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot, Thing thanThisFacility)
		{
			if (facilityDef != thanThisFacility.def)
			{
				Log.Error("Comparing two different facility defs.");
				return false;
			}
			Vector3 b = GenThing.TrueCenter(facilityPos, facilityRot, facilityDef.size, facilityDef.Altitude);
			Vector3 a = this.parent.TrueCenter();
			float num = Vector3.Distance(a, b);
			float num2 = Vector3.Distance(a, thanThisFacility.TrueCenter());
			if (num != num2)
			{
				return num < num2;
			}
			if (facilityPos.x != thanThisFacility.Position.x)
			{
				return facilityPos.x < thanThisFacility.Position.x;
			}
			return facilityPos.z < thanThisFacility.Position.z;
		}

		// Token: 0x17001A12 RID: 6674
		// (get) Token: 0x06009972 RID: 39282 RVA: 0x003781C1 File Offset: 0x003763C1
		private IEnumerable<Thing> ThingsICanLinkTo
		{
			get
			{
				CompAffectedByFacilities.<get_ThingsICanLinkTo>d__22 <get_ThingsICanLinkTo>d__ = new CompAffectedByFacilities.<get_ThingsICanLinkTo>d__22(-2);
				<get_ThingsICanLinkTo>d__.<>4__this = this;
				return <get_ThingsICanLinkTo>d__;
			}
		}

		// Token: 0x06009973 RID: 39283 RVA: 0x003781D1 File Offset: 0x003763D1
		public static IEnumerable<Thing> PotentialThingsToLinkTo(ThingDef myDef, IntVec3 myPos, Rot4 myRot, Map myMap)
		{
			CompAffectedByFacilities.<PotentialThingsToLinkTo>d__24 <PotentialThingsToLinkTo>d__ = new CompAffectedByFacilities.<PotentialThingsToLinkTo>d__24(-2);
			<PotentialThingsToLinkTo>d__.<>3__myDef = myDef;
			<PotentialThingsToLinkTo>d__.<>3__myPos = myPos;
			<PotentialThingsToLinkTo>d__.<>3__myRot = myRot;
			<PotentialThingsToLinkTo>d__.<>3__myMap = myMap;
			return <PotentialThingsToLinkTo>d__;
		}

		// Token: 0x06009974 RID: 39284 RVA: 0x003781F8 File Offset: 0x003763F8
		public static void DrawLinesToPotentialThingsToLinkTo(ThingDef myDef, IntVec3 myPos, Rot4 myRot, Map map)
		{
			Vector3 a = GenThing.TrueCenter(myPos, myRot, myDef.size, myDef.Altitude);
			foreach (Thing t in CompAffectedByFacilities.PotentialThingsToLinkTo(myDef, myPos, myRot, map))
			{
				GenDraw.DrawLineBetween(a, t.TrueCenter());
			}
		}

		// Token: 0x06009975 RID: 39285 RVA: 0x00378264 File Offset: 0x00376464
		public static void DrawPlaceMouseAttachmentsToPotentialThingsToLinkTo(float curX, ref float curY, ThingDef myDef, IntVec3 myPos, Rot4 myRot, Map map)
		{
			CompAffectedByFacilities.<>c__DisplayClass26_0 CS$<>8__locals1;
			CS$<>8__locals1.curX = curX;
			int num = 0;
			foreach (Thing thing in CompAffectedByFacilities.PotentialThingsToLinkTo(myDef, myPos, myRot, map))
			{
				num++;
				if (num == 1)
				{
					CompAffectedByFacilities.<DrawPlaceMouseAttachmentsToPotentialThingsToLinkTo>g__DrawTextLine|26_0(ref curY, "FacilityPotentiallyLinkedTo".Translate() + ":", ref CS$<>8__locals1);
				}
				CompAffectedByFacilities.<DrawPlaceMouseAttachmentsToPotentialThingsToLinkTo>g__DrawTextLine|26_0(ref curY, "  - " + thing.LabelCap, ref CS$<>8__locals1);
			}
		}

		// Token: 0x06009976 RID: 39286 RVA: 0x003782FC File Offset: 0x003764FC
		public void DrawRedLineToPotentiallySupplantedFacility(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot)
		{
			Thing potentiallySupplantedFacility = this.GetPotentiallySupplantedFacility(facilityDef, facilityPos, facilityRot);
			if (potentiallySupplantedFacility != null)
			{
				GenDraw.DrawLineBetween(this.parent.TrueCenter(), potentiallySupplantedFacility.TrueCenter(), CompAffectedByFacilities.InactiveFacilityLineMat, 0.2f);
			}
		}

		// Token: 0x06009977 RID: 39287 RVA: 0x00378338 File Offset: 0x00376538
		private Thing GetPotentiallySupplantedFacility(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot)
		{
			Thing thing = null;
			int num = 0;
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				if (this.linkedFacilities[i].def == facilityDef)
				{
					if (thing == null)
					{
						thing = this.linkedFacilities[i];
					}
					num++;
				}
			}
			if (num == 0)
			{
				return null;
			}
			CompProperties_Facility compProperties = facilityDef.GetCompProperties<CompProperties_Facility>();
			if (num + 1 <= compProperties.maxSimultaneous)
			{
				return null;
			}
			Thing thing2 = thing;
			for (int j = 0; j < this.linkedFacilities.Count; j++)
			{
				if (facilityDef == this.linkedFacilities[j].def && this.IsBetter(thing2.def, thing2.Position, thing2.Rotation, this.linkedFacilities[j]))
				{
					thing2 = this.linkedFacilities[j];
				}
			}
			return thing2;
		}

		// Token: 0x06009978 RID: 39288 RVA: 0x00378410 File Offset: 0x00376610
		public override float GetStatOffset(StatDef stat)
		{
			float num = 0f;
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				CompFacility compFacility = this.linkedFacilities[i].TryGetComp<CompFacility>();
				if (compFacility.StatOffsets != null)
				{
					float statOffsetFromList = compFacility.StatOffsets.GetStatOffsetFromList(stat);
					if (statOffsetFromList != 0f && this.IsFacilityActive(this.linkedFacilities[i]))
					{
						num += statOffsetFromList;
					}
				}
			}
			return num;
		}

		// Token: 0x06009979 RID: 39289 RVA: 0x00378484 File Offset: 0x00376684
		public override void GetStatsExplanation(StatDef stat, StringBuilder sb, string whitespace = "")
		{
			this.alreadyUsed.Clear();
			bool flag = false;
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				bool flag2 = false;
				for (int j = 0; j < this.alreadyUsed.Count; j++)
				{
					if (this.alreadyUsed[j] == this.linkedFacilities[i].def)
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2 && this.IsFacilityActive(this.linkedFacilities[i]))
				{
					CompFacility compFacility = this.linkedFacilities[i].TryGetComp<CompFacility>();
					if (compFacility.StatOffsets != null)
					{
						float num = compFacility.StatOffsets.GetStatOffsetFromList(stat);
						if (num != 0f)
						{
							if (!flag)
							{
								flag = true;
								sb.AppendLine();
								sb.AppendLine(whitespace + "StatsReport_Facilities".Translate() + ":");
							}
							int num2 = 0;
							for (int k = 0; k < this.linkedFacilities.Count; k++)
							{
								if (this.IsFacilityActive(this.linkedFacilities[k]) && this.linkedFacilities[k].def == this.linkedFacilities[i].def)
								{
									num2++;
								}
							}
							num *= (float)num2;
							sb.Append(whitespace + "    ");
							if (num2 != 1)
							{
								sb.Append(num2.ToString() + "x ");
							}
							sb.AppendLine(this.linkedFacilities[i].LabelCap + ": " + num.ToStringByStyle(stat.toStringStyle, ToStringNumberSense.Offset));
							this.alreadyUsed.Add(this.linkedFacilities[i].def);
						}
					}
				}
			}
		}

		// Token: 0x0600997A RID: 39290 RVA: 0x00378660 File Offset: 0x00376860
		private void RelinkAll()
		{
			this.LinkToNearbyFacilities();
		}

		// Token: 0x0600997B RID: 39291 RVA: 0x00378668 File Offset: 0x00376868
		public bool IsFacilityActive(Thing facility)
		{
			return facility.TryGetComp<CompFacility>().CanBeActive;
		}

		// Token: 0x0600997C RID: 39292 RVA: 0x00378678 File Offset: 0x00376878
		private void LinkToNearbyFacilities()
		{
			this.UnlinkAll();
			if (!this.parent.Spawned)
			{
				return;
			}
			foreach (Thing thing in this.ThingsICanLinkTo)
			{
				CompFacility compFacility;
				if (thing.TryGetComp(out compFacility))
				{
					this.linkedFacilities.Add(thing);
					compFacility.Notify_NewLink(this.parent);
				}
			}
		}

		// Token: 0x0600997D RID: 39293 RVA: 0x003786F4 File Offset: 0x003768F4
		private void UnlinkAll()
		{
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				this.linkedFacilities[i].TryGetComp<CompFacility>().Notify_LinkRemoved(this.parent);
			}
			this.linkedFacilities.Clear();
		}

		// Token: 0x06009980 RID: 39296 RVA: 0x00378790 File Offset: 0x00376990
		[CompilerGenerated]
		internal static void <DrawPlaceMouseAttachmentsToPotentialThingsToLinkTo>g__DrawTextLine|26_0(ref float y, string text, ref CompAffectedByFacilities.<>c__DisplayClass26_0 A_2)
		{
			float lineHeight = Text.LineHeight;
			Widgets.Label(new Rect(A_2.curX, y, 999f, lineHeight), text);
			y += lineHeight;
		}

		// Token: 0x04005B88 RID: 23432
		private List<Thing> linkedFacilities = new List<Thing>();

		// Token: 0x04005B89 RID: 23433
		public static Material InactiveFacilityLineMat = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, new Color(1f, 0.5f, 0.5f));

		// Token: 0x04005B8A RID: 23434
		private static readonly Dictionary<ThingDef, int> alreadyReturnedCount = new Dictionary<ThingDef, int>();

		// Token: 0x04005B8B RID: 23435
		private List<ThingDef> alreadyUsed = new List<ThingDef>();
	}
}
