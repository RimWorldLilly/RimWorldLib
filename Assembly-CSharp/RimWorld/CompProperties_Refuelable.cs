using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200194F RID: 6479
	public class CompProperties_Refuelable : CompProperties
	{
		// Token: 0x17001C1E RID: 7198
		// (get) Token: 0x0600A284 RID: 41604 RVA: 0x003A126A File Offset: 0x0039F46A
		public string FuelLabel
		{
			get
			{
				if (this.fuelLabel.NullOrEmpty())
				{
					return "Fuel".TranslateSimple();
				}
				return this.fuelLabel;
			}
		}

		// Token: 0x17001C1F RID: 7199
		// (get) Token: 0x0600A285 RID: 41605 RVA: 0x003A128A File Offset: 0x0039F48A
		public string FuelGizmoLabel
		{
			get
			{
				if (this.fuelGizmoLabel.NullOrEmpty())
				{
					return "Fuel".TranslateSimple();
				}
				return this.fuelGizmoLabel;
			}
		}

		// Token: 0x17001C20 RID: 7200
		// (get) Token: 0x0600A286 RID: 41606 RVA: 0x003A12AC File Offset: 0x0039F4AC
		public Texture2D FuelIcon
		{
			get
			{
				if (this.fuelIcon == null)
				{
					if (!this.fuelIconPath.NullOrEmpty())
					{
						this.fuelIcon = ContentFinder<Texture2D>.Get(this.fuelIconPath, true);
					}
					else
					{
						ThingDef thingDef;
						if (this.fuelFilter.AnyAllowedDef != null)
						{
							thingDef = this.fuelFilter.AnyAllowedDef;
						}
						else
						{
							thingDef = ThingDefOf.Chemfuel;
						}
						this.fuelIcon = thingDef.uiIcon;
					}
				}
				return this.fuelIcon;
			}
		}

		// Token: 0x17001C21 RID: 7201
		// (get) Token: 0x0600A287 RID: 41607 RVA: 0x003A131B File Offset: 0x0039F51B
		public float FuelMultiplierCurrentDifficulty
		{
			get
			{
				if (this.factorByDifficulty)
				{
					Storyteller storyteller = Find.Storyteller;
					if (((storyteller != null) ? storyteller.difficulty : null) != null)
					{
						return this.fuelMultiplier / Find.Storyteller.difficulty.maintenanceCostFactor;
					}
				}
				return this.fuelMultiplier;
			}
		}

		// Token: 0x0600A288 RID: 41608 RVA: 0x003A1358 File Offset: 0x0039F558
		public CompProperties_Refuelable()
		{
			this.compClass = typeof(CompRefuelable);
		}

		// Token: 0x0600A289 RID: 41609 RVA: 0x003A13C3 File Offset: 0x0039F5C3
		public override void ResolveReferences(ThingDef parentDef)
		{
			base.ResolveReferences(parentDef);
			this.fuelFilter.ResolveReferences();
		}

		// Token: 0x0600A28A RID: 41610 RVA: 0x003A13D7 File Offset: 0x0039F5D7
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_Refuelable.<ConfigErrors>d__41 <ConfigErrors>d__ = new CompProperties_Refuelable.<ConfigErrors>d__41(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x0600A28B RID: 41611 RVA: 0x003A13EE File Offset: 0x0039F5EE
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			CompProperties_Refuelable.<SpecialDisplayStats>d__42 <SpecialDisplayStats>d__ = new CompProperties_Refuelable.<SpecialDisplayStats>d__42(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			<SpecialDisplayStats>d__.<>3__req = req;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x04006002 RID: 24578
		public float fuelConsumptionRate = 1f;

		// Token: 0x04006003 RID: 24579
		public float fuelCapacity = 2f;

		// Token: 0x04006004 RID: 24580
		public float initialFuelPercent;

		// Token: 0x04006005 RID: 24581
		public float autoRefuelPercent = 0.3f;

		// Token: 0x04006006 RID: 24582
		public float fuelConsumptionPerTickInRain;

		// Token: 0x04006007 RID: 24583
		public ThingFilter fuelFilter;

		// Token: 0x04006008 RID: 24584
		public bool destroyOnNoFuel;

		// Token: 0x04006009 RID: 24585
		public bool consumeFuelOnlyWhenUsed;

		// Token: 0x0400600A RID: 24586
		public bool consumeFuelOnlyWhenPowered;

		// Token: 0x0400600B RID: 24587
		public bool showFuelGizmo;

		// Token: 0x0400600C RID: 24588
		public bool initialAllowAutoRefuel = true;

		// Token: 0x0400600D RID: 24589
		public bool showAllowAutoRefuelToggle;

		// Token: 0x0400600E RID: 24590
		public bool allowRefuelIfNotEmpty = true;

		// Token: 0x0400600F RID: 24591
		public bool fuelIsMortarBarrel;

		// Token: 0x04006010 RID: 24592
		public bool targetFuelLevelConfigurable;

		// Token: 0x04006011 RID: 24593
		public float initialConfigurableTargetFuelLevel;

		// Token: 0x04006012 RID: 24594
		public bool drawOutOfFuelOverlay = true;

		// Token: 0x04006013 RID: 24595
		public float minimumFueledThreshold;

		// Token: 0x04006014 RID: 24596
		public bool drawFuelGaugeInMap;

		// Token: 0x04006015 RID: 24597
		public bool atomicFueling;

		// Token: 0x04006016 RID: 24598
		private float fuelMultiplier = 1f;

		// Token: 0x04006017 RID: 24599
		public bool factorByDifficulty;

		// Token: 0x04006018 RID: 24600
		[MustTranslate]
		public string fuelLabel;

		// Token: 0x04006019 RID: 24601
		[MustTranslate]
		public string fuelGizmoLabel;

		// Token: 0x0400601A RID: 24602
		[MustTranslate]
		public string outOfFuelMessage;

		// Token: 0x0400601B RID: 24603
		[NoTranslate]
		public string fuelIconPath;

		// Token: 0x0400601C RID: 24604
		public bool externalTicking;

		// Token: 0x0400601D RID: 24605
		public bool hideGizmosIfNotPlayerFaction;

		// Token: 0x0400601E RID: 24606
		public bool functionsInVacuum = true;

		// Token: 0x0400601F RID: 24607
		public bool canEjectFuel;

		// Token: 0x04006020 RID: 24608
		private Texture2D fuelIcon;
	}
}
