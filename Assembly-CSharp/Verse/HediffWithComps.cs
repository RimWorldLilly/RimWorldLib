using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x0200049D RID: 1181
	public class HediffWithComps : Hediff
	{
		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x060020A2 RID: 8354 RVA: 0x000C3578 File Offset: 0x000C1778
		public override string LabelBase
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				int num = 0;
				for (;;)
				{
					int num2 = num;
					List<HediffComp> list = this.comps;
					int? num3 = (list != null) ? new int?(list.Count) : null;
					if (!(num2 < num3.GetValueOrDefault() & num3 != null))
					{
						break;
					}
					string compLabelPrefix = this.comps[num].CompLabelPrefix;
					if (!compLabelPrefix.NullOrEmpty())
					{
						stringBuilder.Append(compLabelPrefix);
						stringBuilder.Append(" ");
					}
					num++;
				}
				stringBuilder.Append(base.LabelBase);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x060020A3 RID: 8355 RVA: 0x000C360C File Offset: 0x000C180C
		public override string LabelInBrackets
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.LabelInBrackets);
				int num = 0;
				for (;;)
				{
					int num2 = num;
					List<HediffComp> list = this.comps;
					int? num3 = (list != null) ? new int?(list.Count) : null;
					if (!(num2 < num3.GetValueOrDefault() & num3 != null))
					{
						break;
					}
					string compLabelInBracketsExtra = this.comps[num].CompLabelInBracketsExtra;
					if (!compLabelInBracketsExtra.NullOrEmpty())
					{
						if (stringBuilder.Length != 0)
						{
							stringBuilder.Append(", ");
						}
						stringBuilder.Append(compLabelInBracketsExtra);
					}
					num++;
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x060020A4 RID: 8356 RVA: 0x000C36A8 File Offset: 0x000C18A8
		public override bool ShouldRemove
		{
			get
			{
				if (this.comps != null)
				{
					for (int i = 0; i < this.comps.Count; i++)
					{
						if (this.comps[i].CompShouldRemove)
						{
							return true;
						}
					}
				}
				return base.ShouldRemove;
			}
		}

		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x060020A5 RID: 8357 RVA: 0x000C36F0 File Offset: 0x000C18F0
		public override bool Visible
		{
			get
			{
				if (this.comps != null)
				{
					for (int i = 0; i < this.comps.Count; i++)
					{
						if (this.comps[i].CompDisallowVisible())
						{
							return false;
						}
					}
				}
				return base.Visible;
			}
		}

		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x060020A6 RID: 8358 RVA: 0x000C3738 File Offset: 0x000C1938
		public override string TipStringExtra
		{
			get
			{
				bool flag = false;
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.TipStringExtra);
				if (this.comps != null)
				{
					for (int i = 0; i < this.comps.Count; i++)
					{
						string compTipStringExtra = this.comps[i].CompTipStringExtra;
						if (!compTipStringExtra.NullOrEmpty())
						{
							if (stringBuilder.Length > 0 && !flag)
							{
								stringBuilder.AppendLine();
								flag = true;
							}
							stringBuilder.AppendLine(compTipStringExtra);
						}
					}
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x060020A7 RID: 8359 RVA: 0x000C37B8 File Offset: 0x000C19B8
		public override string Description
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(base.Description);
				int num = 0;
				for (;;)
				{
					int num2 = num;
					List<HediffComp> list = this.comps;
					int? num3 = (list != null) ? new int?(list.Count) : null;
					if (!(num2 < num3.GetValueOrDefault() & num3 != null))
					{
						break;
					}
					string compDescriptionExtra = this.comps[num].CompDescriptionExtra;
					if (!compDescriptionExtra.NullOrEmpty())
					{
						stringBuilder.Append(" ");
						stringBuilder.Append(compDescriptionExtra);
					}
					num++;
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x060020A8 RID: 8360 RVA: 0x000C3844 File Offset: 0x000C1A44
		public override TextureAndColor StateIcon
		{
			get
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					TextureAndColor compStateIcon = this.comps[i].CompStateIcon;
					if (compStateIcon.HasValue)
					{
						return compStateIcon;
					}
				}
				return TextureAndColor.None;
			}
		}

		// Token: 0x060020A9 RID: 8361 RVA: 0x000C3889 File Offset: 0x000C1A89
		public override IEnumerable<Gizmo> GetGizmos()
		{
			HediffWithComps.<GetGizmos>d__15 <GetGizmos>d__ = new HediffWithComps.<GetGizmos>d__15(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x060020AA RID: 8362 RVA: 0x000C389C File Offset: 0x000C1A9C
		public override void CopyFrom(Hediff other)
		{
			base.CopyFrom(other);
			HediffWithComps hediffWithComps = other as HediffWithComps;
			if (hediffWithComps != null)
			{
				foreach (HediffComp hediffComp in this.comps)
				{
					foreach (HediffComp hediffComp2 in hediffWithComps.comps)
					{
						if (!(hediffComp.GetType() != hediffComp2.GetType()))
						{
							hediffComp.CopyFrom(hediffComp2);
						}
					}
				}
			}
		}

		// Token: 0x060020AB RID: 8363 RVA: 0x000C3954 File Offset: 0x000C1B54
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CompPostPostAdd(dinfo);
				}
			}
		}

		// Token: 0x060020AC RID: 8364 RVA: 0x000C3998 File Offset: 0x000C1B98
		public override void PostRemoved()
		{
			base.PostRemoved();
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CompPostPostRemoved();
				}
			}
		}

		// Token: 0x060020AD RID: 8365 RVA: 0x000C39DC File Offset: 0x000C1BDC
		public override void PostTick()
		{
			if (this.comps == null)
			{
				return;
			}
			float num = 0f;
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].CompPostTick(ref num);
			}
			if (num != 0f)
			{
				this.Severity += num;
			}
		}

		// Token: 0x060020AE RID: 8366 RVA: 0x000C3A38 File Offset: 0x000C1C38
		public override void PostTickInterval(int delta)
		{
			if (this.comps == null)
			{
				return;
			}
			float num = 0f;
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].CompPostTickInterval(ref num, delta);
			}
			if (num != 0f)
			{
				this.Severity += num;
			}
		}

		// Token: 0x060020AF RID: 8367 RVA: 0x000C3A94 File Offset: 0x000C1C94
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.InitializeComps();
			}
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CompExposeData();
				}
			}
		}

		// Token: 0x060020B0 RID: 8368 RVA: 0x000C3AE4 File Offset: 0x000C1CE4
		public override void Tended(float quality, float maxQuality, int batchPosition = 0)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].CompTended(quality, maxQuality, batchPosition);
			}
		}

		// Token: 0x060020B1 RID: 8369 RVA: 0x000C3B1C File Offset: 0x000C1D1C
		public override bool TryMergeWith(Hediff other)
		{
			if (base.TryMergeWith(other))
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CompPostMerged(other);
				}
				return true;
			}
			return false;
		}

		// Token: 0x060020B2 RID: 8370 RVA: 0x000C3B60 File Offset: 0x000C1D60
		public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
		{
			base.Notify_PawnDied(dinfo, culprit);
			for (int i = this.comps.Count - 1; i >= 0; i--)
			{
				this.comps[i].Notify_PawnDied(dinfo, culprit);
			}
		}

		// Token: 0x060020B3 RID: 8371 RVA: 0x000C3BA0 File Offset: 0x000C1DA0
		public override void Notify_PawnKilled()
		{
			base.Notify_PawnKilled();
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].Notify_PawnKilled();
			}
		}

		// Token: 0x060020B4 RID: 8372 RVA: 0x000C3BDC File Offset: 0x000C1DDC
		public override void Notify_KilledPawn(Pawn victim, DamageInfo? dinfo)
		{
			base.Notify_KilledPawn(victim, dinfo);
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].Notify_KilledPawn(victim, dinfo);
			}
		}

		// Token: 0x060020B5 RID: 8373 RVA: 0x000C3C1C File Offset: 0x000C1E1C
		public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.Notify_PawnPostApplyDamage(dinfo, totalDamageDealt);
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].Notify_PawnPostApplyDamage(dinfo, totalDamageDealt);
			}
		}

		// Token: 0x060020B6 RID: 8374 RVA: 0x000C3C5C File Offset: 0x000C1E5C
		public override void ModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].CompModifyChemicalEffect(chem, ref effect);
			}
		}

		// Token: 0x060020B7 RID: 8375 RVA: 0x000C3C94 File Offset: 0x000C1E94
		public override void Notify_PawnUsedVerb(Verb verb, LocalTargetInfo target)
		{
			base.Notify_PawnUsedVerb(verb, target);
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].Notify_PawnUsedVerb(verb, target);
			}
		}

		// Token: 0x060020B8 RID: 8376 RVA: 0x000C3CD4 File Offset: 0x000C1ED4
		public override void Notify_EntropyGained(float baseAmount, float finalAmount, Thing src = null)
		{
			base.Notify_EntropyGained(baseAmount, finalAmount, src);
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].Notify_EntropyGained(baseAmount, finalAmount, src);
			}
		}

		// Token: 0x060020B9 RID: 8377 RVA: 0x000C3D14 File Offset: 0x000C1F14
		public override void Notify_ImplantUsed(string violationSourceName, float detectionChance, int violationSourceLevel = -1)
		{
			base.Notify_ImplantUsed(violationSourceName, detectionChance, violationSourceLevel);
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].Notify_ImplantUsed(violationSourceName, detectionChance, violationSourceLevel);
			}
		}

		// Token: 0x060020BA RID: 8378 RVA: 0x000C3D54 File Offset: 0x000C1F54
		public override void Notify_Spawned()
		{
			base.Notify_Spawned();
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].Notify_Spawned();
			}
		}

		// Token: 0x060020BB RID: 8379 RVA: 0x000C3D90 File Offset: 0x000C1F90
		public override void Notify_SurgicallyRemoved(Pawn surgeon)
		{
			base.Notify_SurgicallyRemoved(surgeon);
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].Notify_SurgicallyRemoved(surgeon);
			}
		}

		// Token: 0x060020BC RID: 8380 RVA: 0x000C3DCC File Offset: 0x000C1FCC
		public override void Notify_SurgicallyReplaced(Pawn surgeon)
		{
			base.Notify_SurgicallyReplaced(surgeon);
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].Notify_SurgicallyReplaced(surgeon);
			}
		}

		// Token: 0x060020BD RID: 8381 RVA: 0x000C3E08 File Offset: 0x000C2008
		public override void PostMake()
		{
			base.PostMake();
			this.InitializeComps();
			for (int i = this.comps.Count - 1; i >= 0; i--)
			{
				try
				{
					this.comps[i].CompPostMake();
				}
				catch (Exception ex)
				{
					string str = "Error in HediffComp.CompPostMake(): ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
					this.comps.RemoveAt(i);
				}
			}
		}

		// Token: 0x060020BE RID: 8382 RVA: 0x000C3E88 File Offset: 0x000C2088
		private void InitializeComps()
		{
			if (this.def.comps != null)
			{
				this.comps = new List<HediffComp>();
				for (int i = 0; i < this.def.comps.Count; i++)
				{
					HediffComp hediffComp = null;
					try
					{
						hediffComp = (HediffComp)Activator.CreateInstance(this.def.comps[i].compClass);
						hediffComp.props = this.def.comps[i];
						hediffComp.parent = this;
						this.comps.Add(hediffComp);
					}
					catch (Exception ex)
					{
						string str = "Could not instantiate or initialize a HediffComp: ";
						Exception ex2 = ex;
						Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
						this.comps.Remove(hediffComp);
					}
				}
			}
		}

		// Token: 0x060020BF RID: 8383 RVA: 0x000C3F5C File Offset: 0x000C215C
		public T GetComp<T>() where T : HediffComp
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				if (this.comps[i] is T)
				{
					return this.comps[i] as T;
				}
			}
			return default(T);
		}

		// Token: 0x060020C0 RID: 8384 RVA: 0x000C3FB4 File Offset: 0x000C21B4
		public override string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(base.DebugString());
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					string str;
					if (this.comps[i].ToString().Contains('_'))
					{
						str = this.comps[i].ToString().Split('_', StringSplitOptions.None)[1];
					}
					else
					{
						str = this.comps[i].ToString();
					}
					stringBuilder.AppendLine("--" + str);
					string text = this.comps[i].CompDebugString();
					if (!text.NullOrEmpty())
					{
						stringBuilder.AppendLine(text.TrimEnd().Indented("    "));
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400192C RID: 6444
		public List<HediffComp> comps = new List<HediffComp>();
	}
}
