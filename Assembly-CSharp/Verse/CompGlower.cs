using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000273 RID: 627
	public class CompGlower : ThingComp
	{
		// Token: 0x17000371 RID: 881
		// (get) Token: 0x0600122F RID: 4655 RVA: 0x000685A8 File Offset: 0x000667A8
		public CompProperties_Glower Props
		{
			get
			{
				return (CompProperties_Glower)this.props;
			}
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06001230 RID: 4656 RVA: 0x000685B8 File Offset: 0x000667B8
		// (set) Token: 0x06001231 RID: 4657 RVA: 0x000685E8 File Offset: 0x000667E8
		public virtual ColorInt GlowColor
		{
			get
			{
				ColorInt? colorInt = this.glowColorOverride;
				if (colorInt == null)
				{
					return this.Props.glowColor;
				}
				return colorInt.GetValueOrDefault();
			}
			set
			{
				this.SetGlowColorInternal(new ColorInt?(value));
			}
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06001232 RID: 4658 RVA: 0x000685F8 File Offset: 0x000667F8
		// (set) Token: 0x06001233 RID: 4659 RVA: 0x00068628 File Offset: 0x00066828
		public virtual float GlowRadius
		{
			get
			{
				float? num = this.glowRadiusOverride;
				if (num == null)
				{
					return this.Props.glowRadius;
				}
				return num.GetValueOrDefault();
			}
			set
			{
				this.glowRadiusOverride = new float?(value);
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06001234 RID: 4660 RVA: 0x00068638 File Offset: 0x00066838
		protected virtual bool ShouldBeLitNow
		{
			get
			{
				if (!this.parent.Spawned)
				{
					return false;
				}
				if (!FlickUtility.WantsToBeOn(this.parent))
				{
					return false;
				}
				IThingGlower thingGlower = this.parent as IThingGlower;
				if (thingGlower != null && !thingGlower.ShouldBeLitNow())
				{
					return false;
				}
				ThingWithComps parent = this.parent;
				if (parent != null)
				{
					foreach (ThingComp thingComp in parent.AllComps)
					{
						IThingGlower thingGlower2 = thingComp as IThingGlower;
						if (thingGlower2 != null && !thingGlower2.ShouldBeLitNow())
						{
							return false;
						}
					}
					return true;
				}
				return true;
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06001235 RID: 4661 RVA: 0x000686E0 File Offset: 0x000668E0
		public bool Glows
		{
			get
			{
				return this.glowOnInt;
			}
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06001236 RID: 4662 RVA: 0x000686E8 File Offset: 0x000668E8
		public bool HasGlowColorOverride
		{
			get
			{
				return this.glowColorOverride != null;
			}
		}

		// Token: 0x06001237 RID: 4663 RVA: 0x000686F8 File Offset: 0x000668F8
		public void UpdateLit(Map map)
		{
			bool shouldBeLitNow = this.ShouldBeLitNow;
			if (this.glowOnInt == shouldBeLitNow)
			{
				return;
			}
			this.glowOnInt = shouldBeLitNow;
			if (!this.glowOnInt)
			{
				map.mapDrawer.MapMeshDirty(this.parent.Position, MapMeshFlagDefOf.Things);
				map.glowGrid.DeRegisterGlower(this);
				return;
			}
			map.mapDrawer.MapMeshDirty(this.parent.Position, MapMeshFlagDefOf.Things);
			map.glowGrid.RegisterGlower(this);
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x0006877E File Offset: 0x0006697E
		public void ForceRegister(Map map)
		{
			map.glowGrid.DeRegisterGlower(this);
			map.mapDrawer.MapMeshDirty(this.parent.Position, MapMeshFlagDefOf.Things);
			map.glowGrid.RegisterGlower(this);
		}

		// Token: 0x06001239 RID: 4665 RVA: 0x000687B8 File Offset: 0x000669B8
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!this.parent.BeingTransportedOnGravship)
			{
				this.RefreshGlower();
			}
		}

		// Token: 0x0600123A RID: 4666 RVA: 0x000687CD File Offset: 0x000669CD
		public override void PostSwapMap()
		{
			this.RefreshGlower();
		}

		// Token: 0x0600123B RID: 4667 RVA: 0x000687D5 File Offset: 0x000669D5
		public override void PostMapInit()
		{
			this.RefreshGlower();
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x000687DD File Offset: 0x000669DD
		private void RefreshGlower()
		{
			this.UpdateLit(this.parent.Map);
			if (this.ShouldBeLitNow)
			{
				this.parent.Map.glowGrid.RegisterGlower(this);
			}
		}

		// Token: 0x0600123D RID: 4669 RVA: 0x00068810 File Offset: 0x00066A10
		public override void ReceiveCompSignal(string signal)
		{
			if (signal == "PowerTurnedOn" || signal == "PowerTurnedOff" || signal == "FlickedOn" || signal == "FlickedOff" || signal == "Refueled" || signal == "RanOutOfFuel" || signal == "ScheduledOn" || signal == "ScheduledOff" || signal == "MechClusterDefeated" || signal == "Hacked" || signal == "RitualTargetChanged" || signal == "CrateContentsChanged" || signal == "Vacuum")
			{
				this.UpdateLit(this.parent.Map);
			}
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x000688E0 File Offset: 0x00066AE0
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.glowOnInt, "glowOn", false, false);
			Scribe_Values.Look<ColorInt?>(ref this.glowColorOverride, "glowColorOverride", null, false);
		}

		// Token: 0x0600123F RID: 4671 RVA: 0x00068919 File Offset: 0x00066B19
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			base.PostDeSpawn(map, mode);
			this.UpdateLit(map);
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x0006892C File Offset: 0x00066B2C
		private List<CompGlower> ExtraSelectedGlowers(CompGlower.GlowerColorChangeType type)
		{
			CompGlower.tmpExtraGlowers.Clear();
			foreach (object obj in Find.Selector.SelectedObjectsListForReading)
			{
				if (obj != this)
				{
					ThingWithComps thingWithComps = obj as ThingWithComps;
					if (thingWithComps != null)
					{
						foreach (CompGlower compGlower in thingWithComps.GetComps<CompGlower>())
						{
							if (type == CompGlower.GlowerColorChangeType.AllGlowers || (type == CompGlower.GlowerColorChangeType.ColorPickerEnabled && compGlower.Props.colorPickerEnabled) || (type == CompGlower.GlowerColorChangeType.DarklightToggle && compGlower.Props.darklightToggle))
							{
								CompGlower.tmpExtraGlowers.Add(compGlower);
							}
						}
					}
				}
			}
			return CompGlower.tmpExtraGlowers;
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x00068A04 File Offset: 0x00066C04
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompGlower.<CompGetGizmosExtra>d__34 <CompGetGizmosExtra>d__ = new CompGlower.<CompGetGizmosExtra>d__34(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x06001242 RID: 4674 RVA: 0x00068A14 File Offset: 0x00066C14
		protected virtual void SetGlowColorInternal(ColorInt? color)
		{
			if (this.ShouldBeLitNow)
			{
				this.parent.MapHeld.glowGrid.DeRegisterGlower(this);
			}
			this.glowColorOverride = color;
			if (this.ShouldBeLitNow)
			{
				this.parent.MapHeld.glowGrid.RegisterGlower(this);
			}
		}

		// Token: 0x04001076 RID: 4214
		public static Widgets.ColorComponents visibleColorTextfields = Widgets.ColorComponents.Hue | Widgets.ColorComponents.Sat;

		// Token: 0x04001077 RID: 4215
		public static Widgets.ColorComponents editableColorTextfields = Widgets.ColorComponents.Hue | Widgets.ColorComponents.Sat;

		// Token: 0x04001078 RID: 4216
		public static Widgets.ColorComponents visibleDebugColorTextfields = Widgets.ColorComponents.All;

		// Token: 0x04001079 RID: 4217
		public static Widgets.ColorComponents editableDebugColorTextfields = Widgets.ColorComponents.Red | Widgets.ColorComponents.Green | Widgets.ColorComponents.Blue | Widgets.ColorComponents.Hue | Widgets.ColorComponents.Sat;

		// Token: 0x0400107A RID: 4218
		public static ColorInt? ColorClipboard;

		// Token: 0x0400107B RID: 4219
		private bool glowOnInt;

		// Token: 0x0400107C RID: 4220
		private ColorInt? glowColorOverride;

		// Token: 0x0400107D RID: 4221
		private float? glowRadiusOverride;

		// Token: 0x0400107E RID: 4222
		private static List<CompGlower> tmpExtraGlowers = new List<CompGlower>(64);

		// Token: 0x020025D6 RID: 9686
		private enum GlowerColorChangeType
		{
			// Token: 0x04009B2D RID: 39725
			None,
			// Token: 0x04009B2E RID: 39726
			ColorPickerEnabled,
			// Token: 0x04009B2F RID: 39727
			DarklightToggle,
			// Token: 0x04009B30 RID: 39728
			AllGlowers
		}
	}
}
