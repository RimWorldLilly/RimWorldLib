using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C6F RID: 7279
	[StaticConstructorOnStartup]
	public abstract class Command_Hide : Command_Toggle
	{
		// Token: 0x170021A6 RID: 8614
		// (get) Token: 0x0600B9DA RID: 47578 RVA: 0x004191A8 File Offset: 0x004173A8
		public IHideable Hideable
		{
			get
			{
				return this.hideable;
			}
		}

		// Token: 0x170021A7 RID: 8615
		// (get) Token: 0x0600B9DB RID: 47579 RVA: 0x004191B0 File Offset: 0x004173B0
		public override string Label
		{
			get
			{
				return this.Hideable.Hidden ? "CommandUnhideLabel".Translate() : "CommandHideLabel".Translate();
			}
		}

		// Token: 0x0600B9DC RID: 47580 RVA: 0x004191DC File Offset: 0x004173DC
		public Command_Hide(IHideable hideable)
		{
			this.hideable = hideable;
			this.icon = ContentFinder<Texture2D>.Get("UI/Commands/HideZone", true);
			this.defaultDesc = "CommandHideZoneDesc".Translate();
			this.hotKey = KeyBindingDefOf.Misc2;
			this.isActive = (() => hideable.Hidden);
			this.toggleAction = delegate()
			{
				hideable.Hidden = !hideable.Hidden;
			};
		}

		// Token: 0x0600B9DD RID: 47581
		protected abstract IEnumerable<FloatMenuOption> GetOptions();

		// Token: 0x170021A8 RID: 8616
		// (get) Token: 0x0600B9DE RID: 47582 RVA: 0x0041925D File Offset: 0x0041745D
		public override IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
		{
			get
			{
				Command_Hide.<get_RightClickFloatMenuOptions>d__8 <get_RightClickFloatMenuOptions>d__ = new Command_Hide.<get_RightClickFloatMenuOptions>d__8(-2);
				<get_RightClickFloatMenuOptions>d__.<>4__this = this;
				return <get_RightClickFloatMenuOptions>d__;
			}
		}

		// Token: 0x0600B9DF RID: 47583 RVA: 0x0041926D File Offset: 0x0041746D
		protected static IEnumerable<FloatMenuOption> ZoneTypeOptions<T>(string label, Texture2D zoneIcon) where T : Zone
		{
			Command_Hide.<ZoneTypeOptions>d__9<T> <ZoneTypeOptions>d__ = new Command_Hide.<ZoneTypeOptions>d__9<T>(-2);
			<ZoneTypeOptions>d__.<>3__label = label;
			<ZoneTypeOptions>d__.<>3__zoneIcon = zoneIcon;
			return <ZoneTypeOptions>d__;
		}

		// Token: 0x0600B9E0 RID: 47584 RVA: 0x00419284 File Offset: 0x00417484
		protected static void ToggleAll(bool hidden)
		{
			foreach (Zone zone in Find.CurrentMap.zoneManager.AllZones)
			{
				zone.Hidden = hidden;
			}
			Command_Hide.TogglePlans(hidden);
		}

		// Token: 0x0600B9E1 RID: 47585 RVA: 0x004192E4 File Offset: 0x004174E4
		protected static void TogglePlans(bool hidden)
		{
			foreach (Plan plan in Find.CurrentMap.planManager.AllPlans)
			{
				plan.Hidden = hidden;
			}
		}

		// Token: 0x0600B9E2 RID: 47586 RVA: 0x00419340 File Offset: 0x00417540
		protected static void ToggleZones<T>(bool hidden) where T : Zone
		{
			foreach (Zone zone in Find.CurrentMap.zoneManager.AllZones)
			{
				if (zone is T)
				{
					zone.Hidden = hidden;
				}
			}
		}

		// Token: 0x040069D9 RID: 27097
		protected readonly IHideable hideable;
	}
}
