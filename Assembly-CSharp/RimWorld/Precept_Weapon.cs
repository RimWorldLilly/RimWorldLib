using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200164F RID: 5711
	public class Precept_Weapon : Precept
	{
		// Token: 0x170018CF RID: 6351
		// (get) Token: 0x06008F70 RID: 36720 RVA: 0x0034B3BF File Offset: 0x003495BF
		public override string TipLabel
		{
			get
			{
				return this.def.issue.LabelCap + ": " + this.def.LabelCap;
			}
		}

		// Token: 0x170018D0 RID: 6352
		// (get) Token: 0x06008F71 RID: 36721 RVA: 0x0034B3F0 File Offset: 0x003495F0
		public override string UIInfoFirstLine
		{
			get
			{
				return "Noble".Translate().CapitalizeFirst() + ": " + this.noble.LabelCap;
			}
		}

		// Token: 0x170018D1 RID: 6353
		// (get) Token: 0x06008F72 RID: 36722 RVA: 0x0034B430 File Offset: 0x00349630
		public override string UIInfoSecondLine
		{
			get
			{
				return "Despised".Translate().CapitalizeFirst() + ": " + this.despised.LabelCap;
			}
		}

		// Token: 0x170018D2 RID: 6354
		// (get) Token: 0x06008F73 RID: 36723 RVA: 0x0034B46E File Offset: 0x0034966E
		public override Color LabelColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x170018D3 RID: 6355
		// (get) Token: 0x06008F74 RID: 36724 RVA: 0x0034B475 File Offset: 0x00349675
		protected HashSet<ThingDef> NobleWeapons
		{
			get
			{
				if (this.nobleWeaponsCached.EnumerableNullOrEmpty<ThingDef>())
				{
					this.nobleWeaponsCached = new HashSet<ThingDef>(from x in DefDatabase<ThingDef>.AllDefs
					where x.IsWeapon && x.PlayerAcquirable && !x.weaponClasses.NullOrEmpty<WeaponClassDef>() && x.weaponClasses.Contains(this.noble)
					select x);
				}
				return this.nobleWeaponsCached;
			}
		}

		// Token: 0x170018D4 RID: 6356
		// (get) Token: 0x06008F75 RID: 36725 RVA: 0x0034B4AB File Offset: 0x003496AB
		protected HashSet<ThingDef> DespisedWeapons
		{
			get
			{
				if (this.despisedWeaponsCached.EnumerableNullOrEmpty<ThingDef>())
				{
					this.despisedWeaponsCached = new HashSet<ThingDef>(from x in DefDatabase<ThingDef>.AllDefs
					where x.IsWeapon && !x.weaponClasses.NullOrEmpty<WeaponClassDef>() && x.weaponClasses.Contains(this.despised)
					select x);
				}
				return this.despisedWeaponsCached;
			}
		}

		// Token: 0x06008F76 RID: 36726 RVA: 0x0034B4E1 File Offset: 0x003496E1
		private void RecacheData()
		{
			this.nobleWeaponsCached = null;
			this.despisedWeaponsCached = null;
			this.iconWeapon = null;
			this.ClearTipCache();
		}

		// Token: 0x06008F77 RID: 36727 RVA: 0x0034B500 File Offset: 0x00349700
		public override void DrawIcon(Rect rect)
		{
			if (this.iconWeapon == null)
			{
				Rand.PushState(this.randomSeed);
				this.iconWeapon = (from w in this.NobleWeapons
				where !w.IsStuff
				select w).RandomElementWithFallback(null);
				Rand.PopState();
			}
			if (this.iconWeapon == null)
			{
				Rand.PushState(this.randomSeed);
				this.iconWeapon = (from w in this.DespisedWeapons
				where !w.IsStuff
				select w).RandomElementWithFallback(null);
				Rand.PopState();
			}
			Widgets.DefIcon(rect, this.iconWeapon, null, 1f, this.ideo.GetStyleFor(this.iconWeapon), false, null, null, null, 1f);
		}

		// Token: 0x06008F78 RID: 36728 RVA: 0x0034B5E8 File Offset: 0x003497E8
		public override string GetTip()
		{
			if (this.tipCached.NullOrEmpty())
			{
				string[] array = new string[9];
				array[0] = base.GetTip();
				array[1] = "\n\n";
				array[2] = base.ColorizeDescTitle("Noble".Translate().CapitalizeFirst());
				array[3] = ":\n";
				array[4] = (from x in this.NobleWeapons
				where x.canGenerateDefaultDesignator
				select x.LabelCap.Resolve()).ToCommaList(false, false);
				array[5] = "\n\n";
				array[6] = base.ColorizeDescTitle("Despised".Translate().CapitalizeFirst());
				array[7] = ":\n";
				array[8] = (from x in this.DespisedWeapons
				where x.canGenerateDefaultDesignator
				select x.LabelCap.Resolve()).ToCommaList(false, false);
				this.tipCached = string.Concat(array);
			}
			return this.tipCached;
		}

		// Token: 0x06008F79 RID: 36729 RVA: 0x0034B72C File Offset: 0x0034992C
		public override IEnumerable<FloatMenuOption> EditFloatMenuOptions()
		{
			Precept_Weapon.<EditFloatMenuOptions>d__20 <EditFloatMenuOptions>d__ = new Precept_Weapon.<EditFloatMenuOptions>d__20(-2);
			<EditFloatMenuOptions>d__.<>4__this = this;
			return <EditFloatMenuOptions>d__;
		}

		// Token: 0x06008F7A RID: 36730 RVA: 0x0034B73C File Offset: 0x0034993C
		public IdeoWeaponDisposition GetDispositionForWeapon(ThingDef td)
		{
			if (this.NobleWeapons.Contains(td))
			{
				return IdeoWeaponDisposition.Noble;
			}
			if (this.DespisedWeapons.Contains(td))
			{
				return IdeoWeaponDisposition.Despised;
			}
			return IdeoWeaponDisposition.None;
		}

		// Token: 0x06008F7B RID: 36731 RVA: 0x0034B760 File Offset: 0x00349960
		public override bool CompatibleWith(Precept other)
		{
			Precept_Weapon wep = other as Precept_Weapon;
			if (wep != null)
			{
				return !this.NobleWeapons.Any((ThingDef x) => wep.DespisedWeapons.Contains(x)) && !this.DespisedWeapons.Any((ThingDef x) => wep.NobleWeapons.Contains(x));
			}
			return base.CompatibleWith(other);
		}

		// Token: 0x06008F7C RID: 36732 RVA: 0x0034B7C4 File Offset: 0x003499C4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<WeaponClassDef>(ref this.noble, "noble");
			Scribe_Defs.Look<WeaponClassDef>(ref this.despised, "despised");
		}

		// Token: 0x06008F7D RID: 36733 RVA: 0x0034B7EC File Offset: 0x003499EC
		public override void CopyTo(Precept other)
		{
			base.CopyTo(other);
			Precept_Weapon precept_Weapon = (Precept_Weapon)other;
			precept_Weapon.noble = this.noble;
			precept_Weapon.despised = this.despised;
		}

		// Token: 0x0400579B RID: 22427
		public WeaponClassDef noble;

		// Token: 0x0400579C RID: 22428
		public WeaponClassDef despised;

		// Token: 0x0400579D RID: 22429
		[Unsaved(false)]
		private ThingDef iconWeapon;

		// Token: 0x0400579E RID: 22430
		[Unsaved(false)]
		private HashSet<ThingDef> nobleWeaponsCached;

		// Token: 0x0400579F RID: 22431
		[Unsaved(false)]
		private HashSet<ThingDef> despisedWeaponsCached;
	}
}
