using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A8D RID: 6797
	public class StorageSettings : IExposable
	{
		// Token: 0x17001E83 RID: 7811
		// (get) Token: 0x0600AB87 RID: 43911 RVA: 0x003C95E5 File Offset: 0x003C77E5
		private IHaulDestination HaulDestinationOwner
		{
			get
			{
				return this.owner as IHaulDestination;
			}
		}

		// Token: 0x17001E84 RID: 7812
		// (get) Token: 0x0600AB88 RID: 43912 RVA: 0x003C95F2 File Offset: 0x003C77F2
		private IHaulSource HaulSourceOwner
		{
			get
			{
				return this.owner as IHaulSource;
			}
		}

		// Token: 0x17001E85 RID: 7813
		// (get) Token: 0x0600AB89 RID: 43913 RVA: 0x003C95FF File Offset: 0x003C77FF
		private ISlotGroupParent SlotGroupParentOwner
		{
			get
			{
				return this.owner as ISlotGroupParent;
			}
		}

		// Token: 0x17001E86 RID: 7814
		// (get) Token: 0x0600AB8A RID: 43914 RVA: 0x003C960C File Offset: 0x003C780C
		// (set) Token: 0x0600AB8B RID: 43915 RVA: 0x003C9614 File Offset: 0x003C7814
		public StoragePriority Priority
		{
			get
			{
				return this.priorityInt;
			}
			set
			{
				this.priorityInt = value;
				if (Current.ProgramState != ProgramState.Playing)
				{
					return;
				}
				StorageGroup storageGroup = this.owner as StorageGroup;
				if (storageGroup == null)
				{
					if (this.HaulDestinationOwner != null && this.HaulDestinationOwner.Map != null)
					{
						this.HaulDestinationOwner.Map.haulDestinationManager.Notify_HaulDestinationChangedPriority();
					}
					if (this.HaulSourceOwner != null && this.HaulSourceOwner.Map != null)
					{
						this.HaulSourceOwner.Map.haulDestinationManager.Notify_HaulDestinationChangedPriority();
						this.HaulSourceOwner.Map.listerHaulables.RecalculateAllInHaulSource(this.HaulSourceOwner);
					}
					if (this.SlotGroupParentOwner != null && this.SlotGroupParentOwner.Map != null)
					{
						this.SlotGroupParentOwner.Map.listerHaulables.RecalcAllInCells(this.SlotGroupParentOwner.AllSlotCells());
					}
					return;
				}
				Map map = storageGroup.Map;
				if (map != null)
				{
					map.haulDestinationManager.Notify_HaulDestinationChangedPriority();
				}
				Map map2 = storageGroup.Map;
				if (map2 != null)
				{
					map2.listerHaulables.RecalcAllInCells(storageGroup.CellsList);
				}
				Map map3 = storageGroup.Map;
				if (map3 == null)
				{
					return;
				}
				map3.listerHaulables.RecalculateAllInHaulSources(storageGroup.HaulSourcesList);
			}
		}

		// Token: 0x0600AB8C RID: 43916 RVA: 0x003C9732 File Offset: 0x003C7932
		public StorageSettings()
		{
			this.filter = new ThingFilter(new Action(this.TryNotifyChanged));
		}

		// Token: 0x0600AB8D RID: 43917 RVA: 0x003C9758 File Offset: 0x003C7958
		public StorageSettings(IStoreSettingsParent owner) : this()
		{
			this.owner = owner;
			if (owner != null)
			{
				StorageSettings parentStoreSettings = owner.GetParentStoreSettings();
				if (parentStoreSettings != null)
				{
					this.priorityInt = parentStoreSettings.priorityInt;
				}
			}
		}

		// Token: 0x0600AB8E RID: 43918 RVA: 0x003C978B File Offset: 0x003C798B
		public void ExposeData()
		{
			Scribe_Values.Look<StoragePriority>(ref this.priorityInt, "priority", StoragePriority.Unstored, false);
			Scribe_Deep.Look<ThingFilter>(ref this.filter, "filter", new object[]
			{
				new Action(this.TryNotifyChanged)
			});
		}

		// Token: 0x0600AB8F RID: 43919 RVA: 0x003C97C4 File Offset: 0x003C79C4
		public void SetFromPreset(StorageSettingsPreset preset)
		{
			this.filter.SetFromPreset(preset);
			this.TryNotifyChanged();
		}

		// Token: 0x0600AB90 RID: 43920 RVA: 0x003C97D8 File Offset: 0x003C79D8
		public void CopyFrom(StorageSettings other)
		{
			this.Priority = other.Priority;
			this.filter.CopyAllowancesFrom(other.filter);
			this.TryNotifyChanged();
		}

		// Token: 0x0600AB91 RID: 43921 RVA: 0x003C9800 File Offset: 0x003C7A00
		public bool AllowedToAccept(Thing t)
		{
			if (!this.filter.Allows(t))
			{
				return false;
			}
			if (this.owner != null)
			{
				StorageSettings parentStoreSettings = this.owner.GetParentStoreSettings();
				if (parentStoreSettings != null && !parentStoreSettings.AllowedToAccept(t))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600AB92 RID: 43922 RVA: 0x003C9840 File Offset: 0x003C7A40
		public bool AllowedToAccept(ThingDef t)
		{
			if (!this.filter.Allows(t))
			{
				return false;
			}
			if (this.owner != null)
			{
				StorageSettings parentStoreSettings = this.owner.GetParentStoreSettings();
				if (parentStoreSettings != null && !parentStoreSettings.AllowedToAccept(t))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600AB93 RID: 43923 RVA: 0x003C9880 File Offset: 0x003C7A80
		private void TryNotifyChanged()
		{
			IStoreSettingsParent storeSettingsParent = this.owner;
			if (storeSettingsParent == null)
			{
				return;
			}
			storeSettingsParent.Notify_SettingsChanged();
		}

		// Token: 0x0600AB94 RID: 43924 RVA: 0x003C9892 File Offset: 0x003C7A92
		public static StorageSettings EverStorableFixedSettings()
		{
			if (StorageSettings.cachedEverStorableFixedSettings == null)
			{
				StorageSettings.cachedEverStorableFixedSettings = new StorageSettings(null)
				{
					filter = ThingFilter.CreateOnlyEverStorableThingFilter()
				};
			}
			return StorageSettings.cachedEverStorableFixedSettings;
		}

		// Token: 0x0600AB95 RID: 43925 RVA: 0x003C98B6 File Offset: 0x003C7AB6
		public static void ResetStaticData()
		{
			StorageSettings.cachedEverStorableFixedSettings = null;
		}

		// Token: 0x040063DE RID: 25566
		private static StorageSettings cachedEverStorableFixedSettings;

		// Token: 0x040063DF RID: 25567
		public IStoreSettingsParent owner;

		// Token: 0x040063E0 RID: 25568
		public ThingFilter filter;

		// Token: 0x040063E1 RID: 25569
		[LoadAlias("priority")]
		private StoragePriority priorityInt = StoragePriority.Normal;
	}
}
