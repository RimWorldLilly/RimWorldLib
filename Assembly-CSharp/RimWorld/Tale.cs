using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020017D9 RID: 6105
	public class Tale : IExposable, ILoadReferenceable
	{
		// Token: 0x170019DD RID: 6621
		// (get) Token: 0x06009858 RID: 39000 RVA: 0x00373FA7 File Offset: 0x003721A7
		public int AgeTicks
		{
			get
			{
				return Find.TickManager.TicksAbs - this.date;
			}
		}

		// Token: 0x170019DE RID: 6622
		// (get) Token: 0x06009859 RID: 39001 RVA: 0x00373FBA File Offset: 0x003721BA
		public int Uses
		{
			get
			{
				return this.uses;
			}
		}

		// Token: 0x170019DF RID: 6623
		// (get) Token: 0x0600985A RID: 39002 RVA: 0x00373FC2 File Offset: 0x003721C2
		public bool Unused
		{
			get
			{
				return this.uses == 0;
			}
		}

		// Token: 0x170019E0 RID: 6624
		// (get) Token: 0x0600985B RID: 39003 RVA: 0x00373FCD File Offset: 0x003721CD
		public virtual Pawn DominantPawn
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170019E1 RID: 6625
		// (get) Token: 0x0600985C RID: 39004 RVA: 0x00373FD0 File Offset: 0x003721D0
		public float InterestLevel
		{
			get
			{
				float num = this.def.baseInterest;
				num /= (float)(1 + this.uses * 3);
				float a = 0f;
				switch (this.def.type)
				{
				case TaleType.Volatile:
					a = 50f;
					break;
				case TaleType.Expirable:
					a = this.def.expireDays;
					break;
				case TaleType.PermanentHistorical:
					a = 50f;
					break;
				}
				float value = (float)(this.AgeTicks / 60000);
				num *= Mathf.InverseLerp(a, 0f, value);
				if (num < 0.01f)
				{
					num = 0.01f;
				}
				return num;
			}
		}

		// Token: 0x170019E2 RID: 6626
		// (get) Token: 0x0600985D RID: 39005 RVA: 0x00374065 File Offset: 0x00372265
		public bool Expired
		{
			get
			{
				return this.Unused && this.def.type == TaleType.Expirable && (float)this.AgeTicks > this.def.expireDays * 60000f;
			}
		}

		// Token: 0x170019E3 RID: 6627
		// (get) Token: 0x0600985E RID: 39006 RVA: 0x0037409B File Offset: 0x0037229B
		public virtual string ShortSummary
		{
			get
			{
				if (!this.customLabel.NullOrEmpty())
				{
					return this.customLabel.CapitalizeFirst();
				}
				return this.def.LabelCap;
			}
		}

		// Token: 0x06009860 RID: 39008 RVA: 0x003740D5 File Offset: 0x003722D5
		public virtual void GenerateTestData()
		{
			if (Find.CurrentMap == null)
			{
				Log.Error("Can't generate test data because there is no map.");
			}
			this.date = Rand.Range(-108000000, -7200000);
			this.surroundings = TaleData_Surroundings.GenerateRandom(Find.CurrentMap);
		}

		// Token: 0x06009861 RID: 39009 RVA: 0x0037410D File Offset: 0x0037230D
		public virtual bool Concerns(Thing th)
		{
			return false;
		}

		// Token: 0x06009862 RID: 39010 RVA: 0x00374110 File Offset: 0x00372310
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<TaleDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.id, "id", 0, false);
			Scribe_Values.Look<int>(ref this.uses, "uses", 0, false);
			Scribe_Values.Look<int>(ref this.date, "date", 0, false);
			Scribe_Deep.Look<TaleData_Surroundings>(ref this.surroundings, "surroundings", Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.customLabel, "customLabel", null, false);
			Scribe_Values.Look<bool>(ref this.hidden, "hidden", false, false);
		}

		// Token: 0x06009863 RID: 39011 RVA: 0x0037419C File Offset: 0x0037239C
		public void Notify_NewlyUsed()
		{
			this.uses++;
		}

		// Token: 0x06009864 RID: 39012 RVA: 0x003741AC File Offset: 0x003723AC
		public void Notify_ReferenceDestroyed()
		{
			if (this.uses == 0)
			{
				Log.Warning("Called reference destroyed method on tale " + ((this != null) ? this.ToString() : null) + " but uses count is 0.");
				return;
			}
			this.uses--;
		}

		// Token: 0x06009865 RID: 39013 RVA: 0x003741E6 File Offset: 0x003723E6
		public virtual void Notify_FactionRemoved(Faction faction)
		{
		}

		// Token: 0x06009866 RID: 39014 RVA: 0x003741E8 File Offset: 0x003723E8
		public IEnumerable<RulePack> GetTextGenerationIncludes()
		{
			Tale.<GetTextGenerationIncludes>d__28 <GetTextGenerationIncludes>d__ = new Tale.<GetTextGenerationIncludes>d__28(-2);
			<GetTextGenerationIncludes>d__.<>4__this = this;
			return <GetTextGenerationIncludes>d__;
		}

		// Token: 0x06009867 RID: 39015 RVA: 0x003741F8 File Offset: 0x003723F8
		public IEnumerable<Rule> GetTextGenerationRules(Dictionary<string, string> outConstants = null)
		{
			Tale.<GetTextGenerationRules>d__29 <GetTextGenerationRules>d__ = new Tale.<GetTextGenerationRules>d__29(-2);
			<GetTextGenerationRules>d__.<>4__this = this;
			<GetTextGenerationRules>d__.<>3__outConstants = outConstants;
			return <GetTextGenerationRules>d__;
		}

		// Token: 0x06009868 RID: 39016 RVA: 0x0037420F File Offset: 0x0037240F
		protected virtual IEnumerable<Rule> SpecialTextGenerationRules(Dictionary<string, string> outConstants)
		{
			return Enumerable.Empty<Rule>();
		}

		// Token: 0x06009869 RID: 39017 RVA: 0x00374216 File Offset: 0x00372416
		public string GetUniqueLoadID()
		{
			return "Tale_" + this.id.ToString();
		}

		// Token: 0x0600986A RID: 39018 RVA: 0x0037422D File Offset: 0x0037242D
		public override int GetHashCode()
		{
			return this.id;
		}

		// Token: 0x0600986B RID: 39019 RVA: 0x00374238 File Offset: 0x00372438
		public override string ToString()
		{
			string str = string.Concat(new string[]
			{
				"(#",
				this.id.ToString(),
				": ",
				this.ShortSummary,
				"(age=",
				((float)this.AgeTicks / 60000f).ToString("F2"),
				" interest=",
				this.InterestLevel.ToString()
			});
			if (this.Unused && this.def.type == TaleType.Expirable)
			{
				str = str + ", expireDays=" + this.def.expireDays.ToString("F2");
			}
			return str + ")";
		}

		// Token: 0x04005B22 RID: 23330
		public TaleDef def;

		// Token: 0x04005B23 RID: 23331
		public int id;

		// Token: 0x04005B24 RID: 23332
		private int uses;

		// Token: 0x04005B25 RID: 23333
		public int date = -1;

		// Token: 0x04005B26 RID: 23334
		public TaleData_Surroundings surroundings;

		// Token: 0x04005B27 RID: 23335
		public string customLabel;

		// Token: 0x04005B28 RID: 23336
		public bool hidden;
	}
}
