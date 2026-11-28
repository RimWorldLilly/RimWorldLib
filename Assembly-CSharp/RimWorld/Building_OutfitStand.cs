using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001A79 RID: 6777
	[StaticConstructorOnStartup]
	public class Building_OutfitStand : Building, IThingHolderEvents<Thing>, IStorageGroupMember, IHaulEnroute, ILoadReferenceable, IHaulDestination, IStoreSettingsParent, IHaulSource, IThingHolder, IApparelSource, ISearchableContents, IBeautyContainer
	{
		// Token: 0x0600AAAC RID: 43692 RVA: 0x003C6580 File Offset: 0x003C4780
		private static void InitGraphics()
		{
			if (Building_OutfitStand.initializedTextures)
			{
				return;
			}
			Building_OutfitStand.initializedTextures = true;
			Building_OutfitStand.baseGraphic = (Graphic_Multi)GraphicDatabase.Get<Graphic_Multi>("Things/Building/OutfitStand/OutfitStand_Base", ShaderDatabase.Cutout, Building_OutfitStand.baseDrawSize, Color.white);
			Building_OutfitStand.bodyGraphic = (Graphic_Multi)GraphicDatabase.Get<Graphic_Multi>("Things/Building/OutfitStand/OutfitStand_Body", ShaderDatabase.Cutout, Building_OutfitStand.bodyDrawSize, Color.white);
			Building_OutfitStand.bodyGraphicChild = (Graphic_Multi)GraphicDatabase.Get<Graphic_Multi>("Things/Building/OutfitStand/OutfitStand_BodyChild", ShaderDatabase.Cutout, Building_OutfitStand.bodyChildDrawSize, Color.white);
			Building_OutfitStand.headGraphic = (Graphic_Multi)GraphicDatabase.Get<Graphic_Multi>("Things/Building/OutfitStand/OutfitStand_Head", ShaderDatabase.Cutout, Building_OutfitStand.headDrawSize, Color.white);
			Building_OutfitStand.swapOutfitIcon = ContentFinder<Texture2D>.Get("UI/Commands/SwapOutfits", true);
		}

		// Token: 0x17001E26 RID: 7718
		// (get) Token: 0x0600AAAD RID: 43693 RVA: 0x003C6637 File Offset: 0x003C4837
		// (set) Token: 0x0600AAAE RID: 43694 RVA: 0x003C663F File Offset: 0x003C483F
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

		// Token: 0x17001E27 RID: 7719
		// (get) Token: 0x0600AAAF RID: 43695 RVA: 0x003C6648 File Offset: 0x003C4848
		public StorageSettings StoreSettings
		{
			get
			{
				StorageGroup storageGroup = this.storageGroup;
				return ((storageGroup != null) ? storageGroup.GetStoreSettings() : null) ?? this.settings;
			}
		}

		// Token: 0x17001E28 RID: 7720
		// (get) Token: 0x0600AAB0 RID: 43696 RVA: 0x003C6666 File Offset: 0x003C4866
		StorageSettings IStorageGroupMember.ParentStoreSettings
		{
			get
			{
				return this.def.building.fixedStorageSettings;
			}
		}

		// Token: 0x17001E29 RID: 7721
		// (get) Token: 0x0600AAB1 RID: 43697 RVA: 0x003C6678 File Offset: 0x003C4878
		StorageSettings IStorageGroupMember.ThingStoreSettings
		{
			get
			{
				return this.settings;
			}
		}

		// Token: 0x17001E2A RID: 7722
		// (get) Token: 0x0600AAB2 RID: 43698 RVA: 0x003C6680 File Offset: 0x003C4880
		string IStorageGroupMember.StorageGroupTag
		{
			get
			{
				return this.def.building.storageGroupTag;
			}
		}

		// Token: 0x17001E2B RID: 7723
		// (get) Token: 0x0600AAB3 RID: 43699 RVA: 0x003C6692 File Offset: 0x003C4892
		bool IStorageGroupMember.DrawConnectionOverlay
		{
			get
			{
				return base.Spawned;
			}
		}

		// Token: 0x17001E2C RID: 7724
		// (get) Token: 0x0600AAB4 RID: 43700 RVA: 0x003C669A File Offset: 0x003C489A
		bool IStorageGroupMember.DrawStorageTab
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001E2D RID: 7725
		// (get) Token: 0x0600AAB5 RID: 43701 RVA: 0x003C669D File Offset: 0x003C489D
		bool IStorageGroupMember.ShowRenameButton
		{
			get
			{
				return base.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x17001E2E RID: 7726
		// (get) Token: 0x0600AAB6 RID: 43702 RVA: 0x003C66AC File Offset: 0x003C48AC
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600AAB7 RID: 43703 RVA: 0x003C66AF File Offset: 0x003C48AF
		public StorageSettings GetStoreSettings()
		{
			StorageGroup storageGroup = this.storageGroup;
			return ((storageGroup != null) ? storageGroup.GetStoreSettings() : null) ?? this.settings;
		}

		// Token: 0x0600AAB8 RID: 43704 RVA: 0x003C66CD File Offset: 0x003C48CD
		StorageSettings IStoreSettingsParent.GetParentStoreSettings()
		{
			return this.def.building.fixedStorageSettings;
		}

		// Token: 0x0600AAB9 RID: 43705 RVA: 0x003C66DF File Offset: 0x003C48DF
		void IStoreSettingsParent.Notify_SettingsChanged()
		{
			if (!base.Spawned)
			{
				return;
			}
			base.MapHeld.listerHaulables.Notify_HaulSourceChanged(this);
		}

		// Token: 0x0600AABA RID: 43706 RVA: 0x003C66FC File Offset: 0x003C48FC
		int IHaulEnroute.SpaceRemainingFor(ThingDef def)
		{
			if (def.IsApparel)
			{
				return (this.GetStoreSettings().AllowedToAccept(def) && this.HasRoomForApparelOfDef(def) > false) ? 1 : 0;
			}
			return (def.IsWeapon && !this.holdingWeaponCached && this.GetStoreSettings().AllowedToAccept(def) > false) ? 1 : 0;
		}

		// Token: 0x0600AABB RID: 43707 RVA: 0x003C6750 File Offset: 0x003C4950
		bool IHaulDestination.Accepts(Thing t)
		{
			if (t is Apparel)
			{
				return this.GetStoreSettings().AllowedToAccept(t) && (this.innerContainer.Contains(t) || this.HasRoomForApparelOfDef(t.def));
			}
			return t.def.IsWeapon && this.GetStoreSettings().AllowedToAccept(t) && (!this.holdingWeaponCached || this.innerContainer.Contains(t));
		}

		// Token: 0x17001E2F RID: 7727
		// (get) Token: 0x0600AABC RID: 43708 RVA: 0x003C67C7 File Offset: 0x003C49C7
		bool IHaulDestination.HaulDestinationEnabled
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001E30 RID: 7728
		// (get) Token: 0x0600AABD RID: 43709 RVA: 0x003C67CA File Offset: 0x003C49CA
		bool IHaulSource.HaulSourceEnabled
		{
			get
			{
				return this.allowRemovingItems;
			}
		}

		// Token: 0x17001E31 RID: 7729
		// (get) Token: 0x0600AABE RID: 43710 RVA: 0x003C67D2 File Offset: 0x003C49D2
		bool IApparelSource.ApparelSourceEnabled
		{
			get
			{
				return this.allowRemovingItems;
			}
		}

		// Token: 0x0600AABF RID: 43711 RVA: 0x003C67DA File Offset: 0x003C49DA
		void IThingHolder.GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.innerContainer);
		}

		// Token: 0x0600AAC0 RID: 43712 RVA: 0x003C67E8 File Offset: 0x003C49E8
		ThingOwner IThingHolder.GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x17001E32 RID: 7730
		// (get) Token: 0x0600AAC1 RID: 43713 RVA: 0x003C67F0 File Offset: 0x003C49F0
		ThingOwner ISearchableContents.SearchableContents
		{
			get
			{
				return this.innerContainer;
			}
		}

		// Token: 0x17001E33 RID: 7731
		// (get) Token: 0x0600AAC2 RID: 43714 RVA: 0x003C67F8 File Offset: 0x003C49F8
		public float BeautyOffset
		{
			get
			{
				if (this.cachedBeauty < 0f)
				{
					bool outdoors = this.GetRoom(RegionType.Set_All).PsychologicallyOutdoors;
					this.cachedBeauty = this.HeldItems.Aggregate(0f, (float beauty, Thing apparel) => beauty + apparel.GetBeauty(outdoors));
				}
				return this.cachedBeauty - 0.001f;
			}
		}

		// Token: 0x0600AAC3 RID: 43715 RVA: 0x003C685C File Offset: 0x003C4A5C
		public void Notify_ItemAdded(Thing thing)
		{
			this.cachedBeauty = -1f;
			if (base.Spawned)
			{
				base.MapHeld.listerHaulables.Notify_AddedThing(thing);
				this.DirtyRoomStats();
			}
			if (!(thing is Apparel) && thing.def.IsWeapon)
			{
				this.holdingWeaponCached = true;
			}
			this.RecacheGraphics();
		}

		// Token: 0x0600AAC4 RID: 43716 RVA: 0x003C68B8 File Offset: 0x003C4AB8
		public void Notify_ItemRemoved(Thing thing)
		{
			this.cachedBeauty = -1f;
			if (base.Spawned)
			{
				base.MapHeld.listerHaulables.Notify_DeSpawned(thing);
				this.DirtyRoomStats();
			}
			if (this.holdingWeaponCached && !(thing is Apparel) && thing.def.IsWeapon)
			{
				this.holdingWeaponCached = false;
			}
			this.RecacheGraphics();
		}

		// Token: 0x17001E34 RID: 7732
		// (get) Token: 0x0600AAC5 RID: 43717 RVA: 0x003C691C File Offset: 0x003C4B1C
		string IBeautyContainer.BeautyOffsetExplanation
		{
			get
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("ContainedApparelBeauty".Translate());
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted(this.BeautyOffset.ToStringWithSign("0.##"));
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x17001E35 RID: 7733
		// (get) Token: 0x0600AAC6 RID: 43718 RVA: 0x003C696D File Offset: 0x003C4B6D
		protected virtual BodyTypeDef BodyTypeDefForRendering { get; } = BodyTypeDefOf.Male;

		// Token: 0x17001E36 RID: 7734
		// (get) Token: 0x0600AAC7 RID: 43719 RVA: 0x003C6975 File Offset: 0x003C4B75
		protected virtual float WeaponDrawDistanceFactor
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x17001E37 RID: 7735
		// (get) Token: 0x0600AAC8 RID: 43720 RVA: 0x003C697C File Offset: 0x003C4B7C
		public ThingWithComps HeldWeapon
		{
			get
			{
				if (!this.holdingWeaponCached)
				{
					return null;
				}
				return this.innerContainer.InnerListForReading.FirstOrDefault((Thing t) => t.def.IsWeapon) as ThingWithComps;
			}
		}

		// Token: 0x0600AAC9 RID: 43721 RVA: 0x003C69BC File Offset: 0x003C4BBC
		public Building_OutfitStand()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x17001E38 RID: 7736
		// (get) Token: 0x0600AACA RID: 43722 RVA: 0x003C69FC File Offset: 0x003C4BFC
		public IReadOnlyList<Thing> HeldItems
		{
			get
			{
				return this.innerContainer.InnerListForReading;
			}
		}

		// Token: 0x0600AACB RID: 43723 RVA: 0x003C6A09 File Offset: 0x003C4C09
		public bool AddApparel(Apparel apparel)
		{
			return this.innerContainer.TryAdd(apparel, true);
		}

		// Token: 0x0600AACC RID: 43724 RVA: 0x003C6A18 File Offset: 0x003C4C18
		public bool RemoveApparel(Apparel apparel)
		{
			return this.innerContainer.Remove(apparel);
		}

		// Token: 0x0600AACD RID: 43725 RVA: 0x003C6A26 File Offset: 0x003C4C26
		public bool RemoveHeldWeapon(Thing weapon)
		{
			return this.holdingWeaponCached && this.HeldWeapon == weapon && this.innerContainer.Remove(weapon);
		}

		// Token: 0x0600AACE RID: 43726 RVA: 0x003C6A49 File Offset: 0x003C4C49
		public bool TryAddHeldWeapon(Thing weapon)
		{
			if (this.holdingWeaponCached)
			{
				return false;
			}
			if (!this.innerContainer.TryAdd(weapon, true))
			{
				return false;
			}
			this.holdingWeaponCached = true;
			return true;
		}

		// Token: 0x0600AACF RID: 43727 RVA: 0x003C6A70 File Offset: 0x003C4C70
		public bool TryDrop(Thing thing, IntVec3 cell, ThingPlaceMode mode, int count, out Thing dropped)
		{
			if (this.innerContainer.TryDrop(thing, cell, base.Map, mode, count, out dropped, null, null))
			{
				return true;
			}
			dropped = null;
			return false;
		}

		// Token: 0x0600AAD0 RID: 43728 RVA: 0x003C6AA0 File Offset: 0x003C4CA0
		private void DirtyRoomStats()
		{
			Room room = this.GetRoom(RegionType.Set_All);
			if (room == null)
			{
				return;
			}
			room.Notify_ContainedThingSpawnedOrDespawned(this);
		}

		// Token: 0x0600AAD1 RID: 43729 RVA: 0x003C6AB5 File Offset: 0x003C4CB5
		public bool CanEverStoreThing(Thing t)
		{
			return this.def.building.fixedStorageSettings.AllowedToAccept(t);
		}

		// Token: 0x0600AAD2 RID: 43730 RVA: 0x003C6AD0 File Offset: 0x003C4CD0
		public bool HasRoomForApparelOfDef(ThingDef t)
		{
			foreach (Thing thing in this.innerContainer)
			{
				Apparel apparel = thing as Apparel;
				if (apparel != null && !ApparelUtility.CanWearTogether(t, apparel.def, BodyDefOf.Human))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600AAD3 RID: 43731 RVA: 0x003C6B40 File Offset: 0x003C4D40
		public bool TryDropThingsToMakeRoomForThingOfDef(ThingDef t)
		{
			if (this.HasRoomForApparelOfDef(t))
			{
				return true;
			}
			List<Apparel> list = (from ap in this.innerContainer.InnerListForReading.OfType<Apparel>()
			where !ApparelUtility.CanWearTogether(t, ap.def, BodyDefOf.Human)
			select ap).ToList<Apparel>();
			if (list.NullOrEmpty<Apparel>())
			{
				return true;
			}
			foreach (Apparel thing in list)
			{
				Thing thing2;
				if (!this.innerContainer.TryDrop(thing, ThingPlaceMode.Near, out thing2, null, null))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600AAD4 RID: 43732 RVA: 0x003C6BF4 File Offset: 0x003C4DF4
		public override void PostMake()
		{
			base.PostMake();
			this.settings = new StorageSettings(this);
			if (this.def.building.defaultStorageSettings != null)
			{
				this.settings.CopyFrom(this.def.building.defaultStorageSettings);
			}
		}

		// Token: 0x0600AAD5 RID: 43733 RVA: 0x003C6C40 File Offset: 0x003C4E40
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (this.storageGroup != null && map != this.storageGroup.Map)
			{
				StorageSettings storeSettings = this.storageGroup.GetStoreSettings();
				this.storageGroup.RemoveMember(this, true);
				this.storageGroup = null;
				this.settings.CopyFrom(storeSettings);
			}
			LongEventHandler.ExecuteWhenFinished(new Action(this.RecacheGraphics));
		}

		// Token: 0x0600AAD6 RID: 43734 RVA: 0x003C6CA8 File Offset: 0x003C4EA8
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode != DestroyMode.Vanish && mode != DestroyMode.WillReplace)
			{
				this.innerContainer.TryDropAll(base.PositionHeld, base.MapHeld, ThingPlaceMode.Near, null, null, true);
			}
			base.DeSpawn(mode);
		}

		// Token: 0x0600AAD7 RID: 43735 RVA: 0x003C6CD5 File Offset: 0x003C4ED5
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			base.Destroy(mode);
			if (this.storageGroup != null)
			{
				StorageGroup storageGroup = this.storageGroup;
				if (storageGroup != null)
				{
					storageGroup.RemoveMember(this, true);
				}
				this.storageGroup = null;
			}
		}

		// Token: 0x0600AAD8 RID: 43736 RVA: 0x003C6D00 File Offset: 0x003C4F00
		public override void Notify_MinifiedThingAboutToBeDestroyed(DestroyMode mode)
		{
			if (mode != DestroyMode.Vanish && mode != DestroyMode.WillReplace)
			{
				this.innerContainer.TryDropAll(base.PositionHeld, base.MapHeld, ThingPlaceMode.Near, null, null, true);
			}
		}

		// Token: 0x0600AAD9 RID: 43737 RVA: 0x003C6D28 File Offset: 0x003C4F28
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (Find.Selector.SingleSelectedThing == this)
			{
				Room room = this.GetRoom(RegionType.Set_All);
				if (room != null && room.ProperRoom)
				{
					room.DrawFieldEdges();
				}
			}
			StorageGroupUtility.DrawSelectionOverlaysFor(this);
		}

		// Token: 0x0600AADA RID: 43738 RVA: 0x003C6D68 File Offset: 0x003C4F68
		private void RecacheGraphics()
		{
			this.cachedApparelGraphicsHeadgear.Clear();
			this.cachedApparelGraphicsNonHeadgear.Clear();
			this.cachedApparelRenderInfoSkipHead = false;
			List<Apparel> list = this.innerContainer.InnerListForReading.OfType<Apparel>().ToList<Apparel>();
			list.SortBy((Apparel a) => a.def.apparel.LastLayer.drawOrder);
			Dictionary<ApparelLayerDef, int> dictionary = new Dictionary<ApparelLayerDef, int>();
			foreach (Apparel apparel in list)
			{
				ApparelLayerDef lastLayer = apparel.def.apparel.LastLayer;
				bool flag = lastLayer == ApparelLayerDefOf.Overhead || lastLayer == ApparelLayerDefOf.EyeCover;
				this.cachedApparelRenderInfoSkipHead = (this.cachedApparelRenderInfoSkipHead || apparel.def.apparel.renderSkipFlags.NotNullAndContains(RenderSkipFlagDefOf.Head));
				ApparelGraphicRecord apparelGraphicRecord;
				if (ApparelGraphicRecordGetter.TryGetGraphicApparel(apparel, this.BodyTypeDefForRendering, false, out apparelGraphicRecord))
				{
					int valueOrDefault = dictionary.GetValueOrDefault(lastLayer, 0);
					dictionary[lastLayer] = valueOrDefault + 1;
					int num = lastLayer.drawOrder / 10 + valueOrDefault;
					DrawData drawData = apparel.def.apparel.drawData;
					float? num2 = (drawData != null) ? new float?(drawData.LayerForRot(base.Rotation, -1f)) : null;
					if (num2 != null)
					{
						float valueOrDefault2 = num2.GetValueOrDefault();
						if (valueOrDefault2 > 0f)
						{
							int num3 = flag ? 70 : 20;
							int num4 = (int)valueOrDefault2 - num3;
							num += num4 * 2;
						}
					}
					Vector3 one = Vector3.one;
					Vector3 zero = Vector3.zero;
					if (apparel.RenderAsPack())
					{
						Vector2 vector = apparel.def.apparel.wornGraphicData.BeltScaleAt(base.Rotation, this.BodyTypeDefForRendering);
						one.x *= vector.x;
						one.z *= vector.y;
						Vector2 vector2 = apparel.def.apparel.wornGraphicData.BeltOffsetAt(base.Rotation, this.BodyTypeDefForRendering);
						zero.x += vector2.x;
						zero.z += vector2.y;
						if (base.Rotation == Rot4.North)
						{
							num = 93;
						}
						else if (base.Rotation == Rot4.South)
						{
							num = -3;
						}
					}
					if (flag)
					{
						this.cachedApparelGraphicsHeadgear.Add(new Building_OutfitStand.CachedGraphicRenderInfo(apparelGraphicRecord.graphic, num, one, zero));
					}
					else
					{
						this.cachedApparelGraphicsNonHeadgear.Add(new Building_OutfitStand.CachedGraphicRenderInfo(apparelGraphicRecord.graphic, num, one, zero));
					}
				}
			}
		}

		// Token: 0x0600AADB RID: 43739 RVA: 0x003C702C File Offset: 0x003C522C
		private Vector3 HeadOffsetAt(Rot4 rotation)
		{
			Vector2 headOffset = this.BodyTypeDefForRendering.headOffset;
			Vector3 result;
			switch (rotation.AsInt)
			{
			case 0:
				result = new Vector3(0f, 0f, headOffset.y);
				break;
			case 1:
				result = new Vector3(headOffset.x, 0f, headOffset.y);
				break;
			case 2:
				result = new Vector3(0f, 0f, headOffset.y);
				break;
			case 3:
				result = new Vector3(-headOffset.x, 0f, headOffset.y);
				break;
			default:
				result = Vector3.zero;
				break;
			}
			return result;
		}

		// Token: 0x0600AADC RID: 43740 RVA: 0x003C70D0 File Offset: 0x003C52D0
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			if (!Building_OutfitStand.initializedTextures)
			{
				Building_OutfitStand.InitGraphics();
			}
			bool flag = this.BodyTypeDefForRendering == BodyTypeDefOf.Child;
			Graphic coloredVersion = Building_OutfitStand.baseGraphic.GetColoredVersion(Building_OutfitStand.baseGraphic.Shader, this.DrawColor, this.DrawColorTwo);
			Vector3 loc = flag ? new Vector3(drawLoc.x, drawLoc.y, drawLoc.z + 0.2f) : drawLoc;
			coloredVersion.Draw(loc, flip ? base.Rotation.Opposite : base.Rotation, this, 0f);
			Graphic_Multi graphic_Multi = flag ? Building_OutfitStand.bodyGraphicChild : Building_OutfitStand.bodyGraphic;
			graphic_Multi.GetColoredVersion(graphic_Multi.Shader, this.DrawColor, this.DrawColorTwo).Draw(drawLoc.WithYOffset(0.05f), flip ? base.Rotation.Opposite : base.Rotation, this, 0f);
			Rot4 rot = flip ? base.Rotation.Opposite : base.Rotation;
			Mesh mesh = MeshPool.GetMeshSetForSize(1.5f, 1.5f).MeshAt(rot);
			foreach (Building_OutfitStand.CachedGraphicRenderInfo cachedGraphicRenderInfo in this.cachedApparelGraphicsNonHeadgear)
			{
				Graphic graphic = cachedGraphicRenderInfo.graphic;
				Vector3 a = drawLoc;
				a.y = AltitudeLayer.Item.AltitudeFor() + PawnRenderUtility.AltitudeForLayer((float)cachedGraphicRenderInfo.layer);
				Material material = graphic.MatAt(rot, null);
				Vector3 b = graphic.DrawOffset(rot) + cachedGraphicRenderInfo.positionOffset;
				Quaternion q = graphic.QuatFromRot(rot);
				Matrix4x4 matrix = Matrix4x4.TRS(a + b, q, cachedGraphicRenderInfo.scale);
				Graphics.DrawMesh(mesh, matrix, material, 0);
			}
			if (!this.cachedApparelRenderInfoSkipHead)
			{
				foreach (Building_OutfitStand.CachedGraphicRenderInfo cachedGraphicRenderInfo2 in this.cachedApparelGraphicsHeadgear)
				{
					Vector3 a2 = drawLoc + this.HeadOffsetAt(rot);
					a2.y = AltitudeLayer.ItemImportant.AltitudeFor() + PawnRenderUtility.AltitudeForLayer((float)cachedGraphicRenderInfo2.layer);
					Graphic graphic2 = cachedGraphicRenderInfo2.graphic;
					Material material2 = graphic2.MatAt(rot, null);
					Vector3 b2 = graphic2.DrawOffset(rot);
					Quaternion q2 = graphic2.QuatFromRot(rot);
					Matrix4x4 matrix2 = Matrix4x4.TRS(a2 + b2, q2, cachedGraphicRenderInfo2.scale);
					Graphics.DrawMesh(mesh, matrix2, material2, 0);
				}
				Graphic coloredVersion2 = Building_OutfitStand.headGraphic.GetColoredVersion(Building_OutfitStand.headGraphic.Shader, this.DrawColor, this.DrawColorTwo);
				Vector3 vector = drawLoc + this.HeadOffsetAt(rot);
				vector = vector.WithY(AltitudeLayer.ItemImportant.AltitudeFor() - 0.05f);
				coloredVersion2.Draw(vector, flip ? base.Rotation.Opposite : base.Rotation, this, 0f);
			}
			if (this.holdingWeaponCached)
			{
				Vector3 drawPos;
				if ((flip ? base.Rotation.Opposite : base.Rotation) == Rot4.North)
				{
					drawPos = drawLoc.WithYOffset(-0.05f);
				}
				else
				{
					drawPos = drawLoc.WithY(AltitudeLayer.ItemImportant.AltitudeFor() + PawnRenderUtility.AltitudeForLayer(90f));
				}
				PawnRenderUtility.DrawCarriedWeapon(this.HeldWeapon, drawPos, flip ? base.Rotation.Opposite : base.Rotation, this.WeaponDrawDistanceFactor);
			}
		}

		// Token: 0x0600AADD RID: 43741 RVA: 0x003C7450 File Offset: 0x003C5650
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			Building_OutfitStand.<GetFloatMenuOptions>d__90 <GetFloatMenuOptions>d__ = new Building_OutfitStand.<GetFloatMenuOptions>d__90(-2);
			<GetFloatMenuOptions>d__.<>4__this = this;
			<GetFloatMenuOptions>d__.<>3__selPawn = selPawn;
			return <GetFloatMenuOptions>d__;
		}

		// Token: 0x0600AADE RID: 43742 RVA: 0x003C7468 File Offset: 0x003C5668
		private FloatMenuOption GetFloatMenuOptionToWear(Pawn selPawn, Apparel apparel)
		{
			string key = "CannotWear";
			string key2 = "ForceWear";
			if (apparel.def.apparel.LastLayer.IsUtilityLayer)
			{
				key = "CannotEquipApparel";
				key2 = "ForceEquipApparel";
			}
			if (!selPawn.CanReach(apparel, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption(key.Translate(apparel.Label, apparel) + ": " + "NoPath".Translate().CapitalizeFirst(), null, apparel, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (apparel.IsBurning())
			{
				return new FloatMenuOption(key.Translate(apparel.Label, apparel) + ": " + "Burning".Translate(), null, apparel, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (selPawn.apparel.WouldReplaceLockedApparel(apparel))
			{
				return new FloatMenuOption(key.Translate(apparel.Label, apparel) + ": " + "WouldReplaceLockedApparel".Translate().CapitalizeFirst(), null, apparel, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (selPawn.IsMutant && selPawn.mutant.Def.disableApparel)
			{
				return new FloatMenuOption(key.Translate(apparel.Label, apparel) + ": " + selPawn.mutant.Def.LabelCap, null, apparel, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!ApparelUtility.HasPartsToWear(selPawn, apparel.def))
			{
				return new FloatMenuOption(key.Translate(apparel.Label, apparel) + ": " + "CannotWearBecauseOfMissingBodyParts".Translate().CapitalizeFirst(), null, apparel, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			string t;
			if (!EquipmentUtility.CanEquip(apparel, selPawn, out t, true))
			{
				return new FloatMenuOption(key.Translate(apparel.Label, apparel) + ": " + t, null, apparel, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			Action <>9__1;
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(key2.Translate(apparel.LabelShort, apparel), delegate()
			{
				Action action;
				if ((action = <>9__1) == null)
				{
					action = (<>9__1 = delegate()
					{
						selPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.Wear, apparel), new JobTag?(JobTag.Misc), false);
					});
				}
				Action action2 = action;
				Apparel apparelReplacedByNewApparel = ApparelUtility.GetApparelReplacedByNewApparel(selPawn, apparel);
				if (apparelReplacedByNewApparel == null || !ModsConfig.BiotechActive || !MechanitorUtility.TryConfirmBandwidthLossFromDroppingThing(selPawn, apparelReplacedByNewApparel, action2))
				{
					action2();
				}
			}, apparel, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), selPawn, this, "ReservedBy", null);
		}

		// Token: 0x0600AADF RID: 43743 RVA: 0x003C7804 File Offset: 0x003C5A04
		private FloatMenuOption GetFloatMenuOptionForForceWear(Pawn selPawn, Apparel apparel)
		{
			string cannotForceTargetText = "CannotForceTargetToWear";
			string key = "ForceTargetToWear";
			if (apparel.def.apparel.LastLayer.IsUtilityLayer)
			{
				cannotForceTargetText = "CannotForceTargetToEquipApparel";
				key = "ForceTargetToEquipApparel";
			}
			if (!selPawn.CanReach(apparel, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption(cannotForceTargetText.Translate(apparel.Label, apparel) + ": " + "NoPath".Translate().CapitalizeFirst(), null, apparel, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (apparel.IsBurning())
			{
				return new FloatMenuOption(cannotForceTargetText.Translate(apparel.Label, apparel) + ": " + "Burning".Translate(), null, apparel, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(key.Translate(apparel.LabelShort, apparel), delegate()
			{
				bool queueOrder = KeyBindingDefOf.QueueOrder.IsDownEvent;
				Find.Targeter.BeginTargeting(TargetingParameters.ForForceWear(selPawn), delegate(LocalTargetInfo target)
				{
					Pawn targetPawn;
					bool queueOrder;
					if (!target.TryGetPawn(out targetPawn))
					{
						if (ModsConfig.OdysseyActive)
						{
							Building_OutfitStand building_OutfitStand = target.Thing as Building_OutfitStand;
							if (building_OutfitStand != null)
							{
								if (!building_OutfitStand.CanEverStoreThing(apparel))
								{
									Messages.Message("CannotStoreThingOnTarget".Translate(apparel.Named("THING"), building_OutfitStand.Named("TARGET")), MessageTypeDefOf.RejectInput, false);
									return;
								}
								Pawn_JobTracker jobs = selPawn.jobs;
								Job job = JobMaker.MakeJob(JobDefOf.PutApparelOnOutfitStand, apparel, building_OutfitStand);
								queueOrder = queueOrder;
								jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), queueOrder);
							}
						}
						return;
					}
					if (targetPawn.apparel.WouldReplaceLockedApparel(apparel))
					{
						Messages.Message(cannotForceTargetText.Translate(apparel.Label, apparel) + ": " + "WouldReplaceLockedApparel".Translate().CapitalizeFirst(), targetPawn, MessageTypeDefOf.RejectInput, false);
						return;
					}
					if (targetPawn.IsMutant && targetPawn.mutant.Def.disableApparel)
					{
						Messages.Message(cannotForceTargetText.Translate(apparel.Label, apparel) + ": " + targetPawn.mutant.Def.LabelCap, targetPawn, MessageTypeDefOf.RejectInput, false);
						return;
					}
					if (!ApparelUtility.HasPartsToWear(targetPawn, apparel.def))
					{
						Messages.Message(cannotForceTargetText.Translate(apparel.Label, apparel) + ": " + "CannotWearBecauseOfMissingBodyParts".Translate().CapitalizeFirst(), targetPawn, MessageTypeDefOf.RejectInput, false);
						return;
					}
					string t;
					if (!EquipmentUtility.CanEquip(apparel, targetPawn, out t, true))
					{
						Messages.Message(cannotForceTargetText.Translate(apparel.Label, apparel) + ": " + t, targetPawn, MessageTypeDefOf.RejectInput, false);
						return;
					}
					Action action = delegate()
					{
						Pawn_JobTracker jobs2 = selPawn.jobs;
						Job job2 = JobMaker.MakeJob(JobDefOf.ForceTargetWear, targetPawn, apparel);
						bool queueOrder2 = queueOrder;
						jobs2.TryTakeOrderedJob(job2, new JobTag?(JobTag.Misc), queueOrder2);
					};
					Apparel apparelReplacedByNewApparel = ApparelUtility.GetApparelReplacedByNewApparel(targetPawn, apparel);
					if (apparelReplacedByNewApparel == null || !ModsConfig.BiotechActive || !MechanitorUtility.TryConfirmBandwidthLossFromDroppingThing(targetPawn, apparelReplacedByNewApparel, action))
					{
						action();
					}
				}, null, null, null, true);
			}, apparel, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), selPawn, this, "ReservedBy", null);
		}

		// Token: 0x0600AAE0 RID: 43744 RVA: 0x003C79C0 File Offset: 0x003C5BC0
		private FloatMenuOption GetFloatMenuOptionToEquipWeapon(Pawn selPawn, Thing weapon)
		{
			if (!weapon.HasComp<CompEquippable>())
			{
				return null;
			}
			string labelShort = weapon.LabelShort;
			if (weapon.def.IsWeapon && selPawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				return new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "IsIncapableOfViolenceLower".Translate(selPawn.LabelShort, selPawn), null, weapon, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (weapon.def.IsRangedWeapon && selPawn.WorkTagIsDisabled(WorkTags.Shooting))
			{
				return new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "IsIncapableOfShootingLower".Translate(selPawn), null, weapon, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!selPawn.CanReach(weapon, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "NoPath".Translate().CapitalizeFirst(), null, weapon, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!selPawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				return new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "Incapable".Translate().CapitalizeFirst(), null, weapon, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (weapon.IsBurning())
			{
				return new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "BurningLower".Translate(), null, weapon, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (selPawn.IsQuestLodger() && !EquipmentUtility.QuestLodgerCanEquip(weapon, selPawn))
			{
				return new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "QuestRelated".Translate().CapitalizeFirst(), null, weapon, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			string str;
			if (!EquipmentUtility.CanEquip(weapon, selPawn, out str, false))
			{
				return new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + str.CapitalizeFirst(), null, weapon, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			string text = "Equip".Translate(labelShort);
			if (weapon.def.IsRangedWeapon && selPawn.story != null && selPawn.story.traits.HasTrait(TraitDefOf.Brawler))
			{
				text += " " + "EquipWarningBrawler".Translate();
			}
			if (EquipmentUtility.AlreadyBondedToWeapon(weapon, selPawn))
			{
				text += " " + "BladelinkAlreadyBonded".Translate();
				TaggedString dialogText = "BladelinkAlreadyBondedDialog".Translate(selPawn.Named("PAWN"), weapon.Named("WEAPON"), selPawn.equipment.bondedWeapon.Named("BONDEDWEAPON"));
				return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, delegate()
				{
					Find.WindowStack.Add(new Dialog_MessageBox(dialogText, null, null, null, null, null, false, null, null, WindowLayer.Dialog));
				}, weapon, Color.white, MenuOptionPriority.High, null, null, 0f, null, null, true, 0), selPawn, weapon, "ReservedBy", null);
			}
			Action <>9__3;
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, delegate()
			{
				string personaWeaponConfirmationText = EquipmentUtility.GetPersonaWeaponConfirmationText(weapon, selPawn);
				if (!personaWeaponConfirmationText.NullOrEmpty())
				{
					WindowStack windowStack = Find.WindowStack;
					TaggedString text2 = personaWeaponConfirmationText;
					string buttonAText = "Yes".Translate();
					Action buttonAAction;
					if ((buttonAAction = <>9__3) == null)
					{
						buttonAAction = (<>9__3 = delegate()
						{
							base.<GetFloatMenuOptionToEquipWeapon>g__Equip|2();
						});
					}
					windowStack.Add(new Dialog_MessageBox(text2, buttonAText, buttonAAction, "No".Translate(), null, null, false, null, null, WindowLayer.Dialog));
					return;
				}
				base.<GetFloatMenuOptionToEquipWeapon>g__Equip|2();
			}, weapon, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), selPawn, this, "ReservedBy", null);
		}

		// Token: 0x0600AAE1 RID: 43745 RVA: 0x003C7E8D File Offset: 0x003C608D
		private void SetAllowHauling(bool allow)
		{
			if (this.allowRemovingItems == allow)
			{
				return;
			}
			this.allowRemovingItems = allow;
			base.Map.listerHaulables.RecalculateAllInHaulSource(this);
		}

		// Token: 0x0600AAE2 RID: 43746 RVA: 0x003C7EB4 File Offset: 0x003C60B4
		private string GetContentsString()
		{
			return "Contents".Translate() + ": " + (from a in this.innerContainer.InnerListForReading
			select a.LabelNoParenthesis).ToCommaList(false, false).CapitalizeFirst();
		}

		// Token: 0x0600AAE3 RID: 43747 RVA: 0x003C7F1C File Offset: 0x003C611C
		public override string GetInspectString()
		{
			string text = base.GetInspectString() ?? "";
			if (this.storageGroup != null)
			{
				string str = text;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 2);
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("StorageGroupLabel".Translate());
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted(this.storageGroup.RenamableLabel.CapitalizeFirst());
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				text = str + defaultInterpolatedStringHandler.ToStringAndClear();
				if (this.storageGroup.MemberCount > 1)
				{
					text += "(" + "NumBuildings".Translate(this.storageGroup.MemberCount).CapitalizeFirst() + ")";
				}
				else
				{
					text += "(" + "OneBuilding".Translate() + ")";
				}
			}
			if (!this.innerContainer.Any)
			{
				return text;
			}
			if (text.Length > 0)
			{
				text += "\n";
			}
			return text + this.GetContentsString();
		}

		// Token: 0x0600AAE4 RID: 43748 RVA: 0x003C8049 File Offset: 0x003C6249
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_OutfitStand.<GetGizmos>d__97 <GetGizmos>d__ = new Building_OutfitStand.<GetGizmos>d__97(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600AAE5 RID: 43749 RVA: 0x003C805C File Offset: 0x003C625C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThingOwner<Thing>>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Deep.Look<StorageSettings>(ref this.settings, "settings", new object[]
			{
				this
			});
			Scribe_References.Look<StorageGroup>(ref this.storageGroup, "storageGroup", false);
			Scribe_Values.Look<bool>(ref this.allowRemovingItems, "allowRemovingItems", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				foreach (Thing thing in this.innerContainer.InnerListForReading)
				{
					if (!(thing is Apparel) && thing.def.IsWeapon)
					{
						this.holdingWeaponCached = true;
						break;
					}
				}
			}
		}

		// Token: 0x040063A7 RID: 25511
		private static readonly Vector2 baseDrawSize = new Vector2(1.4f, 1.4f);

		// Token: 0x040063A8 RID: 25512
		private static readonly Vector2 bodyDrawSize = new Vector2(1.2f, 1.2f);

		// Token: 0x040063A9 RID: 25513
		private static readonly Vector2 bodyChildDrawSize = new Vector2(1.2f, 1.2f);

		// Token: 0x040063AA RID: 25514
		private static readonly Vector2 headDrawSize = new Vector2(1.3f, 1.3f);

		// Token: 0x040063AB RID: 25515
		private static bool initializedTextures;

		// Token: 0x040063AC RID: 25516
		private static Graphic_Multi baseGraphic;

		// Token: 0x040063AD RID: 25517
		private static Graphic_Multi bodyGraphic;

		// Token: 0x040063AE RID: 25518
		private static Graphic_Multi bodyGraphicChild;

		// Token: 0x040063AF RID: 25519
		private static Graphic_Multi headGraphic;

		// Token: 0x040063B0 RID: 25520
		private static Texture2D swapOutfitIcon;

		// Token: 0x040063B1 RID: 25521
		private ThingOwner<Thing> innerContainer;

		// Token: 0x040063B2 RID: 25522
		private StorageSettings settings;

		// Token: 0x040063B3 RID: 25523
		private StorageGroup storageGroup;

		// Token: 0x040063B4 RID: 25524
		private bool allowRemovingItems;

		// Token: 0x040063B5 RID: 25525
		private bool cachedApparelRenderInfoSkipHead;

		// Token: 0x040063B6 RID: 25526
		private readonly List<Building_OutfitStand.CachedGraphicRenderInfo> cachedApparelGraphicsHeadgear = new List<Building_OutfitStand.CachedGraphicRenderInfo>();

		// Token: 0x040063B7 RID: 25527
		private readonly List<Building_OutfitStand.CachedGraphicRenderInfo> cachedApparelGraphicsNonHeadgear = new List<Building_OutfitStand.CachedGraphicRenderInfo>();

		// Token: 0x040063B8 RID: 25528
		private Building_OutfitStand.CachedGraphicRenderInfo? cachedHeldWeaponGraphic;

		// Token: 0x040063B9 RID: 25529
		private bool holdingWeaponCached;

		// Token: 0x040063BA RID: 25530
		private float cachedBeauty = -1f;

		// Token: 0x02003774 RID: 14196
		private struct CachedGraphicRenderInfo
		{
			// Token: 0x0601445C RID: 83036 RVA: 0x0068780F File Offset: 0x00685A0F
			public CachedGraphicRenderInfo(Graphic graphic, int layer, Vector3 scale, Vector3 positionOffset)
			{
				this.graphic = graphic;
				this.layer = layer;
				this.scale = scale;
				this.positionOffset = positionOffset;
			}

			// Token: 0x0400DD87 RID: 56711
			public Graphic graphic;

			// Token: 0x0400DD88 RID: 56712
			public int layer;

			// Token: 0x0400DD89 RID: 56713
			public Vector3 scale;

			// Token: 0x0400DD8A RID: 56714
			public Vector3 positionOffset;
		}
	}
}
