using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001A5A RID: 6746
	[StaticConstructorOnStartup]
	public class Frame : Building, IThingHolder, IConstructible, IStorageGroupMember, IStoreSettingsParent, IHaulEnroute, ILoadReferenceable
	{
		// Token: 0x17001DBB RID: 7611
		// (get) Token: 0x0600A944 RID: 43332 RVA: 0x003C0380 File Offset: 0x003BE580
		public ThingDef BuildDef
		{
			get
			{
				return this.def.entityDefToBuild as ThingDef;
			}
		}

		// Token: 0x17001DBC RID: 7612
		// (get) Token: 0x0600A945 RID: 43333 RVA: 0x003C0392 File Offset: 0x003BE592
		public float WorkToBuild
		{
			get
			{
				return this.def.entityDefToBuild.GetStatValueAbstract(StatDefOf.WorkToBuild, base.Stuff);
			}
		}

		// Token: 0x17001DBD RID: 7613
		// (get) Token: 0x0600A946 RID: 43334 RVA: 0x003C03AF File Offset: 0x003BE5AF
		public float WorkLeft
		{
			get
			{
				return this.WorkToBuild - this.workDone;
			}
		}

		// Token: 0x17001DBE RID: 7614
		// (get) Token: 0x0600A947 RID: 43335 RVA: 0x003C03BE File Offset: 0x003BE5BE
		public float PercentComplete
		{
			get
			{
				return this.workDone / this.WorkToBuild;
			}
		}

		// Token: 0x17001DBF RID: 7615
		// (get) Token: 0x0600A948 RID: 43336 RVA: 0x003C03CD File Offset: 0x003BE5CD
		public override string Label
		{
			get
			{
				return this.LabelEntityToBuild + "FrameLabelExtra".Translate();
			}
		}

		// Token: 0x17001DC0 RID: 7616
		// (get) Token: 0x0600A949 RID: 43337 RVA: 0x003C03EC File Offset: 0x003BE5EC
		public string LabelEntityToBuild
		{
			get
			{
				string text = this.def.entityDefToBuild.label;
				if (base.StyleSourcePrecept != null)
				{
					text = base.StyleSourcePrecept.TransformThingLabel(text);
				}
				if (base.Stuff != null)
				{
					return "ThingMadeOfStuffLabel".Translate(base.Stuff.LabelAsStuff, text);
				}
				return text;
			}
		}

		// Token: 0x17001DC1 RID: 7617
		// (get) Token: 0x0600A94A RID: 43338 RVA: 0x003C0450 File Offset: 0x003BE650
		public override Color DrawColor
		{
			get
			{
				if (!this.def.MadeFromStuff)
				{
					List<ThingDefCountClass> costList = this.def.entityDefToBuild.CostList;
					if (costList != null)
					{
						for (int i = 0; i < costList.Count; i++)
						{
							ThingDef thingDef = costList[i].thingDef;
							if (thingDef.IsStuff && thingDef.stuffProps.color != Color.white)
							{
								return this.def.GetColorForStuff(thingDef);
							}
						}
					}
					return new Color(0.6f, 0.6f, 0.6f);
				}
				return base.DrawColor;
			}
		}

		// Token: 0x17001DC2 RID: 7618
		// (get) Token: 0x0600A94B RID: 43339 RVA: 0x003C04E4 File Offset: 0x003BE6E4
		public EffecterDef ConstructionEffect
		{
			get
			{
				ThingDef stuff = base.Stuff;
				if (((stuff != null) ? stuff.stuffProps.constructEffect : null) != null)
				{
					return base.Stuff.stuffProps.constructEffect;
				}
				if (this.def.entityDefToBuild.constructEffect != null)
				{
					return this.def.entityDefToBuild.constructEffect;
				}
				return EffecterDefOf.ConstructMetal;
			}
		}

		// Token: 0x17001DC3 RID: 7619
		// (get) Token: 0x0600A94C RID: 43340 RVA: 0x003C0543 File Offset: 0x003BE743
		private Material CornerMat
		{
			get
			{
				if (this.cachedCornerMat == null)
				{
					this.cachedCornerMat = MaterialPool.MatFrom(Frame.CornerTex, ShaderDatabase.MetaOverlay, this.DrawColor);
				}
				return this.cachedCornerMat;
			}
		}

		// Token: 0x17001DC4 RID: 7620
		// (get) Token: 0x0600A94D RID: 43341 RVA: 0x003C0574 File Offset: 0x003BE774
		private Material TileMat
		{
			get
			{
				if (this.cachedTileMat == null)
				{
					this.cachedTileMat = MaterialPool.MatFrom(Frame.TileTex, ShaderDatabase.MetaOverlay, this.DrawColor);
				}
				return this.cachedTileMat;
			}
		}

		// Token: 0x0600A94E RID: 43342 RVA: 0x003C05A5 File Offset: 0x003BE7A5
		public Frame()
		{
			this.resourceContainer = new ThingOwner<Thing>(this, false, LookMode.Deep, true);
		}

		// Token: 0x0600A94F RID: 43343 RVA: 0x003C05BC File Offset: 0x003BE7BC
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.resourceContainer;
		}

		// Token: 0x0600A950 RID: 43344 RVA: 0x003C05C4 File Offset: 0x003BE7C4
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0600A951 RID: 43345 RVA: 0x003C05D4 File Offset: 0x003BE7D4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workDone, "workDone", 0f, false);
			Scribe_Deep.Look<ThingOwner>(ref this.resourceContainer, "resourceContainer", new object[]
			{
				this
			});
			Scribe_Values.Look<ColorInt?>(ref this.glowerColorOverride, "glowerColorOverride", null, false);
			Scribe_References.Look<StorageGroup>(ref this.storageGroup, "storageGroup", false);
			Scribe_Deep.Look<StorageSettings>(ref this.storageSettings, "storageSettings", Array.Empty<object>());
		}

		// Token: 0x0600A952 RID: 43346 RVA: 0x003C0658 File Offset: 0x003BE858
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			bool spawned = base.Spawned;
			Map map = base.Map;
			base.Destroy(mode);
			if (spawned)
			{
				Blueprint_Storage blueprint_Storage = ThingUtility.CheckAutoRebuildOnDestroyed(this, mode, map, this.def.entityDefToBuild) as Blueprint_Storage;
				if (blueprint_Storage != null)
				{
					blueprint_Storage.SetStorageGroup(this.storageGroup, true);
					blueprint_Storage.settings = new StorageSettings();
					blueprint_Storage.settings.CopyFrom(this.storageSettings);
				}
			}
		}

		// Token: 0x0600A953 RID: 43347 RVA: 0x003C06C0 File Offset: 0x003BE8C0
		public ThingDef EntityToBuildStuff()
		{
			return base.Stuff;
		}

		// Token: 0x0600A954 RID: 43348 RVA: 0x003C06C8 File Offset: 0x003BE8C8
		public List<ThingDefCountClass> TotalMaterialCost()
		{
			return this.def.entityDefToBuild.CostListAdjusted(base.Stuff, true);
		}

		// Token: 0x0600A955 RID: 43349 RVA: 0x003C06E4 File Offset: 0x003BE8E4
		public bool IsCompleted()
		{
			foreach (ThingDefCountClass thingDefCountClass in this.TotalMaterialCost())
			{
				if (thingDefCountClass.count - this.resourceContainer.TotalStackCountOfDef(thingDefCountClass.thingDef) > 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600A956 RID: 43350 RVA: 0x003C0754 File Offset: 0x003BE954
		public int ThingCountNeeded(ThingDef stuff)
		{
			foreach (ThingDefCountClass thingDefCountClass in this.TotalMaterialCost())
			{
				if (thingDefCountClass.thingDef == stuff)
				{
					return thingDefCountClass.count - this.resourceContainer.TotalStackCountOfDef(thingDefCountClass.thingDef);
				}
			}
			return 0;
		}

		// Token: 0x0600A957 RID: 43351 RVA: 0x003C07C8 File Offset: 0x003BE9C8
		public int ThingCountNeededWithEnroute(ThingDef stuff, Pawn excludeEnrouteFor = null)
		{
			foreach (ThingDefCountClass thingDefCountClass in this.TotalMaterialCost())
			{
				if (thingDefCountClass.thingDef == stuff)
				{
					int num = thingDefCountClass.count - this.resourceContainer.TotalStackCountOfDef(thingDefCountClass.thingDef);
					int num2 = num - base.Map.enrouteManager.GetEnroute(this, thingDefCountClass.thingDef, excludeEnrouteFor);
					if (num2 < 0)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 3);
						defaultInterpolatedStringHandler.AppendLiteral("amount of stuff for ");
						defaultInterpolatedStringHandler.AppendFormatted(this.LabelShort);
						defaultInterpolatedStringHandler.AppendLiteral(" is negative: ");
						defaultInterpolatedStringHandler.AppendFormatted(stuff.label);
						defaultInterpolatedStringHandler.AppendLiteral(" ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(num2);
						defaultInterpolatedStringHandler.AppendLiteral(".");
						Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					if (num2 > num)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(66, 4);
						defaultInterpolatedStringHandler2.AppendLiteral("amount of stuff for ");
						defaultInterpolatedStringHandler2.AppendFormatted(this.LabelShort);
						defaultInterpolatedStringHandler2.AppendLiteral(" was greater than could be needed: ");
						defaultInterpolatedStringHandler2.AppendFormatted(stuff.label);
						defaultInterpolatedStringHandler2.AppendLiteral(" ");
						defaultInterpolatedStringHandler2.AppendFormatted<int>(num2);
						defaultInterpolatedStringHandler2.AppendLiteral(" (needs ");
						defaultInterpolatedStringHandler2.AppendFormatted<int>(num);
						defaultInterpolatedStringHandler2.AppendLiteral(").");
						Log.Error(defaultInterpolatedStringHandler2.ToStringAndClear());
					}
					return Mathf.Min(Mathf.Max(num2, 0), num);
				}
			}
			return 0;
		}

		// Token: 0x0600A958 RID: 43352 RVA: 0x003C0968 File Offset: 0x003BEB68
		public void CompleteConstruction(Pawn worker)
		{
			if (worker.Faction != null)
			{
				QuestUtility.SendQuestTargetSignals(worker.Faction.questTags, "BuiltBuilding", this.Named("SUBJECT"));
			}
			List<CompHasSources> list = new List<CompHasSources>();
			for (int i = 0; i < this.resourceContainer.Count; i++)
			{
				CompHasSources compHasSources = this.resourceContainer[i].TryGetComp<CompHasSources>();
				if (compHasSources != null)
				{
					list.Add(compHasSources);
				}
			}
			this.resourceContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			Map map = base.Map;
			bool flag = Find.Selector.IsSelected(this);
			this.Destroy(DestroyMode.Vanish);
			if (this.GetStatValue(StatDefOf.WorkToBuild, true, -1) > 150f)
			{
				ThingDef thingDef = this.def.entityDefToBuild as ThingDef;
				if (thingDef != null && thingDef.category == ThingCategory.Building)
				{
					SoundDefOf.Building_Complete.PlayOneShot(new TargetInfo(base.Position, map, false));
				}
			}
			ThingDef thingDef2 = this.def.entityDefToBuild as ThingDef;
			Thing thing = null;
			if (thingDef2 != null)
			{
				thing = ThingMaker.MakeThing(thingDef2, base.Stuff);
				thing.SetFactionDirect(base.Faction);
				CompQuality compQuality = thing.TryGetComp<CompQuality>();
				if (compQuality != null)
				{
					QualityCategory q = QualityUtility.GenerateQualityCreatedByPawn(worker, SkillDefOf.Construction, true);
					compQuality.SetQuality(q, new ArtGenerationContext?(ArtGenerationContext.Colony));
					QualityUtility.SendCraftNotification(thing, worker);
				}
				CompArt compArt = thing.TryGetComp<CompArt>();
				if (compArt != null)
				{
					if (compQuality == null)
					{
						compArt.InitializeArt(ArtGenerationContext.Colony);
					}
					compArt.JustCreatedBy(worker);
				}
				CompHasSources compHasSources2 = thing.TryGetComp<CompHasSources>();
				if (compHasSources2 != null && !list.NullOrEmpty<CompHasSources>())
				{
					for (int j = 0; j < list.Count; j++)
					{
						list[j].TransferSourcesTo(compHasSources2, -1);
					}
				}
				if (this.GetIdeoForStyle(worker) != null)
				{
					thing.StyleDef = this.StyleDef;
				}
				thing.HitPoints = Mathf.CeilToInt((float)this.HitPoints / (float)base.MaxHitPoints * (float)thing.MaxHitPoints);
				GenSpawn.Spawn(thing, base.Position, map, base.Rotation, WipeMode.FullRefund, false, false);
				Building building = thing as Building;
				if (building != null)
				{
					Lord lord = worker.GetLord();
					if (lord != null)
					{
						lord.AddBuilding(building);
					}
					building.StyleSourcePrecept = base.StyleSourcePrecept;
				}
				IStorageGroupMember storageGroupMember = thing as IStorageGroupMember;
				if (storageGroupMember != null)
				{
					storageGroupMember.SetStorageGroup(this.storageGroup, true);
					storageGroupMember.ThingStoreSettings.CopyFrom(this.storageSettings);
				}
				this.SetStorageGroup(null, false);
				if (thingDef2 != null)
				{
					Color? ideoColorForBuilding = IdeoUtility.GetIdeoColorForBuilding(thingDef2, base.Faction);
					if (ideoColorForBuilding != null)
					{
						thing.SetColor(ideoColorForBuilding.Value, true);
					}
				}
				if (this.overrideGraphicIndex != null)
				{
					thing.overrideGraphicIndex = this.overrideGraphicIndex;
				}
			}
			else
			{
				TerrainDef terrainDef = map.terrainGrid.TerrainAt(base.Position);
				TerrainDef terrainDef2 = (TerrainDef)this.def.entityDefToBuild;
				if (terrainDef.tempTerrain != null && terrainDef.tempTerrain.replaceableByBridge && terrainDef2.bridge)
				{
					map.terrainGrid.RemoveTempTerrain(base.Position, false, false);
				}
				if (terrainDef2.isFoundation)
				{
					map.terrainGrid.SetFoundation(base.Position, terrainDef2);
				}
				else if (terrainDef2.temporary)
				{
					map.terrainGrid.SetTempTerrain(base.Position, terrainDef2);
				}
				else
				{
					map.terrainGrid.SetTerrain(base.Position, terrainDef2);
				}
				FilthMaker.RemoveAllFilth(base.Position, map);
			}
			worker.records.Increment(RecordDefOf.ThingsConstructed);
			if (thing != null && thing.GetStatValue(StatDefOf.WorkToBuild, true, -1) >= 9500f)
			{
				TaleRecorder.RecordTale(TaleDefOf.CompletedLongConstructionProject, new object[]
				{
					worker,
					thing.def
				});
			}
			if (thing != null && flag)
			{
				Find.Selector.Select(thing, false, false);
			}
			if (this.glowerColorOverride != null)
			{
				CompGlower compGlower = (thing != null) ? thing.TryGetComp<CompGlower>() : null;
				if (compGlower != null)
				{
					compGlower.GlowColor = this.glowerColorOverride.Value;
				}
			}
			Lord lord2 = worker.GetLord();
			if (lord2 != null)
			{
				Building building2 = thing as Building;
				if (building2 != null)
				{
					lord2.Notify_ConstructionCompleted(worker, building2);
				}
			}
		}

		// Token: 0x0600A959 RID: 43353 RVA: 0x003C0D80 File Offset: 0x003BEF80
		private Ideo GetIdeoForStyle(Pawn worker)
		{
			if (worker.Ideo != null)
			{
				return worker.Ideo;
			}
			if (ModsConfig.BiotechActive && worker.IsColonyMech)
			{
				Pawn overseer = worker.GetOverseer();
				if (((overseer != null) ? overseer.Ideo : null) != null)
				{
					return overseer.Ideo;
				}
			}
			return null;
		}

		// Token: 0x0600A95A RID: 43354 RVA: 0x003C0DC8 File Offset: 0x003BEFC8
		public void FailConstruction(Pawn worker)
		{
			Map map = base.Map;
			this.Destroy(DestroyMode.FailConstruction);
			Blueprint_Build blueprint_Build = null;
			if (this.def.entityDefToBuild.blueprintDef != null)
			{
				blueprint_Build = (Blueprint_Build)ThingMaker.MakeThing(this.def.entityDefToBuild.blueprintDef, null);
				blueprint_Build.stuffToUse = base.Stuff;
				blueprint_Build.SetFactionDirect(base.Faction);
				blueprint_Build.InheritStyle(base.StyleSourcePrecept, this.StyleDef);
				IStorageGroupMember storageGroupMember = blueprint_Build as IStorageGroupMember;
				if (storageGroupMember != null)
				{
					storageGroupMember.Group = this.storageGroup;
				}
				Blueprint_Storage blueprint_Storage = blueprint_Build as Blueprint_Storage;
				if (blueprint_Storage != null && this.storageSettings != null)
				{
					blueprint_Storage.SetStorageGroup(this.storageGroup, true);
					blueprint_Storage.settings = new StorageSettings();
					blueprint_Storage.settings.CopyFrom(this.storageSettings);
				}
				GenSpawn.Spawn(blueprint_Build, base.Position, map, base.Rotation, WipeMode.FullRefund, false, false);
			}
			Lord lord = worker.GetLord();
			if (lord != null)
			{
				lord.Notify_ConstructionFailed(worker, this, blueprint_Build);
			}
			MoteMaker.ThrowText(this.DrawPos, map, "TextMote_ConstructionFail".Translate(), 6f);
			if (base.Faction == Faction.OfPlayer && this.WorkToBuild > 1400f)
			{
				Messages.Message("MessageConstructionFailed".Translate(this.LabelEntityToBuild, worker.LabelShort, worker.Named("WORKER")), new TargetInfo(base.Position, map, false), MessageTypeDefOf.NegativeEvent, true);
			}
		}

		// Token: 0x0600A95B RID: 43355 RVA: 0x003C0F48 File Offset: 0x003BF148
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			if (WorldComponent_GravshipController.CutsceneInProgress && !WorldComponent_GravshipController.GravshipRenderInProgess)
			{
				return;
			}
			Vector3 vector = drawLoc;
			ThingDef buildDef = this.BuildDef;
			bool? flag;
			if (buildDef == null)
			{
				flag = null;
			}
			else
			{
				BuildingProperties building = buildDef.building;
				flag = ((building != null) ? new bool?(building.isAttachment) : null);
			}
			bool? flag2 = flag;
			if (flag2.GetValueOrDefault())
			{
				vector += (base.Rotation.AsVector2 * 0.5f).ToVector3();
			}
			Vector2 vector2 = new Vector2((float)this.def.size.x, (float)this.def.size.z);
			vector2.x *= 1.15f;
			vector2.y *= 1.15f;
			Vector3 s = new Vector3(vector2.x, 1f, vector2.y);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(vector, base.Rotation.AsQuat, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, Frame.UnderfieldMat, 0);
			int num = 4;
			for (int i = 0; i < num; i++)
			{
				float num2 = (float)Mathf.Min(base.RotatedSize.x, base.RotatedSize.z) * 0.38f;
				IntVec3 intVec = default(IntVec3);
				if (i == 0)
				{
					intVec = new IntVec3(-1, 0, -1);
				}
				else if (i == 1)
				{
					intVec = new IntVec3(-1, 0, 1);
				}
				else if (i == 2)
				{
					intVec = new IntVec3(1, 0, 1);
				}
				else if (i == 3)
				{
					intVec = new IntVec3(1, 0, -1);
				}
				Vector3 b = default(Vector3);
				b.x = (float)intVec.x * ((float)base.RotatedSize.x / 2f - num2 / 2f);
				b.z = (float)intVec.z * ((float)base.RotatedSize.z / 2f - num2 / 2f);
				Vector3 s2 = new Vector3(num2, 1f, num2);
				Matrix4x4 matrix2 = default(Matrix4x4);
				matrix2.SetTRS(vector + Vector3.up * 0.03f + b, new Rot4(i).AsQuat, s2);
				Graphics.DrawMesh(MeshPool.plane10, matrix2, this.CornerMat, 0);
			}
			int num3 = Mathf.CeilToInt((this.PercentComplete - 0f) / 1f * (float)base.RotatedSize.x * (float)base.RotatedSize.z * 4f);
			IntVec2 intVec2 = base.RotatedSize * 2;
			for (int j = 0; j < num3; j++)
			{
				IntVec2 intVec3 = default(IntVec2);
				intVec3.z = j / intVec2.x;
				intVec3.x = j - intVec3.z * intVec2.x;
				Vector3 a = new Vector3((float)intVec3.x * 0.5f, 0f, (float)intVec3.z * 0.5f) + vector;
				a.x -= (float)base.RotatedSize.x * 0.5f - 0.25f;
				a.z -= (float)base.RotatedSize.z * 0.5f - 0.25f;
				Vector3 s3 = new Vector3(0.5f, 1f, 0.5f);
				Matrix4x4 matrix3 = default(Matrix4x4);
				matrix3.SetTRS(a + Vector3.up * 0.02f, Quaternion.identity, s3);
				Graphics.DrawMesh(MeshPool.plane10, matrix3, this.TileMat, 0);
			}
			base.Comps_PostDraw();
		}

		// Token: 0x0600A95C RID: 43356 RVA: 0x003C12FE File Offset: 0x003BF4FE
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Frame.<GetGizmos>d__46 <GetGizmos>d__ = new Frame.<GetGizmos>d__46(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600A95D RID: 43357 RVA: 0x003C1310 File Offset: 0x003BF510
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			stringBuilder.AppendLineIfNotEmpty();
			stringBuilder.AppendLine("ContainedResources".Translate() + ":");
			List<ThingDefCountClass> list = this.def.entityDefToBuild.CostListAdjusted(base.Stuff, true);
			for (int i = 0; i < list.Count; i++)
			{
				ThingDefCountClass thingDefCountClass = list[i];
				int value = thingDefCountClass.count - this.ThingCountNeeded(thingDefCountClass.thingDef);
				StringBuilder stringBuilder2 = stringBuilder;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 3);
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(thingDefCountClass.thingDef.LabelCap);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(value);
				defaultInterpolatedStringHandler.AppendLiteral(" / ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(thingDefCountClass.count);
				stringBuilder2.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			stringBuilder.Append("WorkLeft".Translate() + ": " + this.WorkLeft.ToStringWorkAmount());
			ThingStyleDef styleDef = this.StyleDef;
			if (((styleDef != null) ? styleDef.Category : null) != null && base.StyleSourcePrecept == null)
			{
				stringBuilder.AppendInNewLine("Style".Translate() + ": " + this.StyleDef.Category.LabelCap);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x17001DC5 RID: 7621
		// (get) Token: 0x0600A95E RID: 43358 RVA: 0x003C147E File Offset: 0x003BF67E
		// (set) Token: 0x0600A95F RID: 43359 RVA: 0x003C1486 File Offset: 0x003BF686
		StorageGroup IStorageGroupMember.Group
		{
			get
			{
				return this.storageGroup;
			}
			set
			{
				this.storageGroup = value;
			}
		}

		// Token: 0x17001DC6 RID: 7622
		// (get) Token: 0x0600A960 RID: 43360 RVA: 0x003C148F File Offset: 0x003BF68F
		Map IStorageGroupMember.Map
		{
			get
			{
				return base.Map;
			}
		}

		// Token: 0x0600A961 RID: 43361 RVA: 0x003C1497 File Offset: 0x003BF697
		public bool Accepts(Thing t)
		{
			return this.ThingCountNeeded(t.def) > 0;
		}

		// Token: 0x0600A962 RID: 43362 RVA: 0x003C14A8 File Offset: 0x003BF6A8
		public int SpaceRemainingFor(ThingDef stuff)
		{
			return this.ThingCountNeeded(stuff);
		}

		// Token: 0x17001DC7 RID: 7623
		// (get) Token: 0x0600A963 RID: 43363 RVA: 0x003C14B1 File Offset: 0x003BF6B1
		StorageSettings IStorageGroupMember.StoreSettings
		{
			get
			{
				return this.GetStoreSettings();
			}
		}

		// Token: 0x17001DC8 RID: 7624
		// (get) Token: 0x0600A964 RID: 43364 RVA: 0x003C14B9 File Offset: 0x003BF6B9
		StorageSettings IStorageGroupMember.ParentStoreSettings
		{
			get
			{
				return this.GetParentStoreSettings();
			}
		}

		// Token: 0x17001DC9 RID: 7625
		// (get) Token: 0x0600A965 RID: 43365 RVA: 0x003C14C1 File Offset: 0x003BF6C1
		StorageSettings IStorageGroupMember.ThingStoreSettings
		{
			get
			{
				return this.storageSettings;
			}
		}

		// Token: 0x17001DCA RID: 7626
		// (get) Token: 0x0600A966 RID: 43366 RVA: 0x003C14C9 File Offset: 0x003BF6C9
		public string StorageGroupTag
		{
			get
			{
				ThingDef buildDef = this.BuildDef;
				if (buildDef == null)
				{
					return null;
				}
				BuildingProperties building = buildDef.building;
				if (building == null)
				{
					return null;
				}
				return building.storageGroupTag;
			}
		}

		// Token: 0x17001DCB RID: 7627
		// (get) Token: 0x0600A967 RID: 43367 RVA: 0x003C14E7 File Offset: 0x003BF6E7
		bool IStorageGroupMember.DrawConnectionOverlay
		{
			get
			{
				return !this.StorageGroupTag.NullOrEmpty();
			}
		}

		// Token: 0x17001DCC RID: 7628
		// (get) Token: 0x0600A968 RID: 43368 RVA: 0x003C14F7 File Offset: 0x003BF6F7
		bool IStorageGroupMember.ShowRenameButton
		{
			get
			{
				return !this.StorageGroupTag.NullOrEmpty() && base.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x17001DCD RID: 7629
		// (get) Token: 0x0600A969 RID: 43369 RVA: 0x003C1515 File Offset: 0x003BF715
		public bool DrawStorageTab
		{
			get
			{
				return !this.StorageGroupTag.NullOrEmpty();
			}
		}

		// Token: 0x17001DCE RID: 7630
		// (get) Token: 0x0600A96A RID: 43370 RVA: 0x003C1525 File Offset: 0x003BF725
		bool IStoreSettingsParent.StorageTabVisible
		{
			get
			{
				return !this.StorageGroupTag.NullOrEmpty();
			}
		}

		// Token: 0x0600A96B RID: 43371 RVA: 0x003C1535 File Offset: 0x003BF735
		public StorageSettings GetStoreSettings()
		{
			if (this.storageGroup != null)
			{
				return this.storageGroup.GetStoreSettings();
			}
			return this.storageSettings;
		}

		// Token: 0x0600A96C RID: 43372 RVA: 0x003C1551 File Offset: 0x003BF751
		public StorageSettings GetParentStoreSettings()
		{
			ThingDef buildDef = this.BuildDef;
			if (buildDef == null)
			{
				return null;
			}
			BuildingProperties building = buildDef.building;
			if (building == null)
			{
				return null;
			}
			return building.fixedStorageSettings;
		}

		// Token: 0x0600A96D RID: 43373 RVA: 0x003C156F File Offset: 0x003BF76F
		void IStoreSettingsParent.Notify_SettingsChanged()
		{
		}

		// Token: 0x0600A96E RID: 43374 RVA: 0x003C1571 File Offset: 0x003BF771
		public override IEnumerable<InspectTabBase> GetInspectTabs()
		{
			Frame.<GetInspectTabs>d__74 <GetInspectTabs>d__ = new Frame.<GetInspectTabs>d__74(-2);
			<GetInspectTabs>d__.<>4__this = this;
			return <GetInspectTabs>d__;
		}

		// Token: 0x0400631C RID: 25372
		public ThingOwner resourceContainer;

		// Token: 0x0400631D RID: 25373
		public float workDone;

		// Token: 0x0400631E RID: 25374
		public ColorInt? glowerColorOverride;

		// Token: 0x0400631F RID: 25375
		public StorageGroup storageGroup;

		// Token: 0x04006320 RID: 25376
		public StorageSettings storageSettings;

		// Token: 0x04006321 RID: 25377
		private Material cachedCornerMat;

		// Token: 0x04006322 RID: 25378
		private Material cachedTileMat;

		// Token: 0x04006323 RID: 25379
		protected const float UnderfieldOverdrawFactor = 1.15f;

		// Token: 0x04006324 RID: 25380
		private const int LongConstructionProjectThreshold = 9500;

		// Token: 0x04006325 RID: 25381
		private static readonly Material UnderfieldMat = MaterialPool.MatFrom("Things/Building/BuildingFrame/Underfield", ShaderDatabase.Transparent);

		// Token: 0x04006326 RID: 25382
		private static readonly Texture2D CornerTex = ContentFinder<Texture2D>.Get("Things/Building/BuildingFrame/Corner", true);

		// Token: 0x04006327 RID: 25383
		private static readonly Texture2D TileTex = ContentFinder<Texture2D>.Get("Things/Building/BuildingFrame/Tile", true);
	}
}
