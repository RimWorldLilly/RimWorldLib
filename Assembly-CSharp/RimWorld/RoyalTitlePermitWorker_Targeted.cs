using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200150E RID: 5390
	public abstract class RoyalTitlePermitWorker_Targeted : RoyalTitlePermitWorker, ITargetingSource
	{
		// Token: 0x17001666 RID: 5734
		// (get) Token: 0x06008412 RID: 33810 RVA: 0x002F7D6F File Offset: 0x002F5F6F
		public bool CasterIsPawn
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001667 RID: 5735
		// (get) Token: 0x06008413 RID: 33811 RVA: 0x002F7D72 File Offset: 0x002F5F72
		public bool IsMeleeAttack
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001668 RID: 5736
		// (get) Token: 0x06008414 RID: 33812 RVA: 0x002F7D75 File Offset: 0x002F5F75
		public bool Targetable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001669 RID: 5737
		// (get) Token: 0x06008415 RID: 33813 RVA: 0x002F7D78 File Offset: 0x002F5F78
		public bool MultiSelect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700166A RID: 5738
		// (get) Token: 0x06008416 RID: 33814 RVA: 0x002F7D7B File Offset: 0x002F5F7B
		public bool HidePawnTooltips
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700166B RID: 5739
		// (get) Token: 0x06008417 RID: 33815 RVA: 0x002F7D7E File Offset: 0x002F5F7E
		public Thing Caster
		{
			get
			{
				return this.caller;
			}
		}

		// Token: 0x1700166C RID: 5740
		// (get) Token: 0x06008418 RID: 33816 RVA: 0x002F7D86 File Offset: 0x002F5F86
		public Pawn CasterPawn
		{
			get
			{
				return this.caller;
			}
		}

		// Token: 0x1700166D RID: 5741
		// (get) Token: 0x06008419 RID: 33817 RVA: 0x002F7D8E File Offset: 0x002F5F8E
		public Verb GetVerb
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700166E RID: 5742
		// (get) Token: 0x0600841A RID: 33818 RVA: 0x002F7D91 File Offset: 0x002F5F91
		public Texture2D UIIcon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700166F RID: 5743
		// (get) Token: 0x0600841B RID: 33819 RVA: 0x002F7D94 File Offset: 0x002F5F94
		public TargetingParameters targetParams
		{
			get
			{
				return this.targetingParameters;
			}
		}

		// Token: 0x17001670 RID: 5744
		// (get) Token: 0x0600841C RID: 33820 RVA: 0x002F7D9C File Offset: 0x002F5F9C
		public ITargetingSource DestinationSelector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001671 RID: 5745
		// (get) Token: 0x0600841D RID: 33821 RVA: 0x002F7D9F File Offset: 0x002F5F9F
		protected float RangeClamped
		{
			get
			{
				return Mathf.Min(this.def.royalAid.targetingRange, this.map.weatherManager.CurWeatherMaxRangeCap);
			}
		}

		// Token: 0x0600841E RID: 33822 RVA: 0x002F7DC8 File Offset: 0x002F5FC8
		public bool CanHitTarget(LocalTargetInfo target)
		{
			if (this.def.royalAid.targetingRequireLOS && !GenSight.LineOfSight(this.caller.Position, target.Cell, this.map, true, null, 0, 0))
			{
				bool flag = false;
				ShootLeanUtility.LeanShootingSourcesFromTo(this.caller.Position, target.Cell, this.map, RoyalTitlePermitWorker_Targeted.tempSourceList);
				for (int i = 0; i < RoyalTitlePermitWorker_Targeted.tempSourceList.Count; i++)
				{
					if (GenSight.LineOfSight(RoyalTitlePermitWorker_Targeted.tempSourceList[i], target.Cell, this.map, true, null, 0, 0))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600841F RID: 33823 RVA: 0x002F7E74 File Offset: 0x002F6074
		public virtual bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			if (!this.CanHitTarget(target))
			{
				if (target.IsValid)
				{
					Messages.Message(this.def.LabelCap + ": " + "AbilityCannotHitTarget".Translate(), MessageTypeDefOf.RejectInput, true);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06008420 RID: 33824 RVA: 0x002F7ECA File Offset: 0x002F60CA
		public virtual void DrawHighlight(LocalTargetInfo target)
		{
			GenDraw.DrawRadiusRing(this.caller.Position, this.RangeClamped, Color.white, null);
			if (target.IsValid)
			{
				GenDraw.DrawTargetHighlight(target);
			}
		}

		// Token: 0x06008421 RID: 33825 RVA: 0x002F7EF7 File Offset: 0x002F60F7
		public virtual void OrderForceTarget(LocalTargetInfo target)
		{
		}

		// Token: 0x06008422 RID: 33826 RVA: 0x002F7EFC File Offset: 0x002F60FC
		public virtual void OnGUI(LocalTargetInfo target)
		{
			Texture2D icon;
			if (target.IsValid)
			{
				if (this.UIIcon != null && this.UIIcon != BaseContent.BadTex)
				{
					icon = this.UIIcon;
				}
				else
				{
					icon = TexCommand.Attack;
				}
			}
			else
			{
				icon = TexCommand.CannotShoot;
			}
			GenUI.DrawMouseAttachment(icon);
		}

		// Token: 0x04005187 RID: 20871
		protected bool free;

		// Token: 0x04005188 RID: 20872
		protected Pawn caller;

		// Token: 0x04005189 RID: 20873
		protected Map map;

		// Token: 0x0400518A RID: 20874
		protected TargetingParameters targetingParameters;

		// Token: 0x0400518B RID: 20875
		private static List<IntVec3> tempSourceList = new List<IntVec3>();
	}
}
