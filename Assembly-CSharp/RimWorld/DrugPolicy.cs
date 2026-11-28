using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020015B4 RID: 5556
	public class DrugPolicy : Policy
	{
		// Token: 0x170017B9 RID: 6073
		// (get) Token: 0x060089A6 RID: 35238 RVA: 0x0031D9EC File Offset: 0x0031BBEC
		public int Count
		{
			get
			{
				return this.entriesInt.Count;
			}
		}

		// Token: 0x170017BA RID: 6074
		// (get) Token: 0x060089A7 RID: 35239 RVA: 0x0031D9F9 File Offset: 0x0031BBF9
		protected override string LoadKey
		{
			get
			{
				return "DrugPolicy";
			}
		}

		// Token: 0x170017BB RID: 6075
		public DrugPolicyEntry this[int index]
		{
			get
			{
				return this.entriesInt[index];
			}
			set
			{
				this.entriesInt[index] = value;
			}
		}

		// Token: 0x170017BC RID: 6076
		public DrugPolicyEntry this[ThingDef drugDef]
		{
			get
			{
				for (int i = 0; i < this.entriesInt.Count; i++)
				{
					if (this.entriesInt[i].drug == drugDef)
					{
						return this.entriesInt[i];
					}
				}
				throw new ArgumentException();
			}
		}

		// Token: 0x060089AB RID: 35243 RVA: 0x0031DA69 File Offset: 0x0031BC69
		public DrugPolicy()
		{
		}

		// Token: 0x060089AC RID: 35244 RVA: 0x0031DA71 File Offset: 0x0031BC71
		public DrugPolicy(int id, string label) : base(id, label)
		{
			this.InitializeIfNeeded(true);
		}

		// Token: 0x060089AD RID: 35245 RVA: 0x0031DA84 File Offset: 0x0031BC84
		private void InitializeIfNeeded(bool overwriteExisting = true)
		{
			if (overwriteExisting)
			{
				if (this.entriesInt != null)
				{
					return;
				}
				this.entriesInt = new List<DrugPolicyEntry>();
			}
			List<ThingDef> thingDefs = DefDatabase<ThingDef>.AllDefsListForReading;
			int i;
			Predicate<DrugPolicyEntry> <>9__1;
			int j;
			for (i = 0; i < thingDefs.Count; i = j + 1)
			{
				if (thingDefs[i].category == ThingCategory.Item && thingDefs[i].IsDrug)
				{
					if (!overwriteExisting)
					{
						List<DrugPolicyEntry> list = this.entriesInt;
						Predicate<DrugPolicyEntry> predicate;
						if ((predicate = <>9__1) == null)
						{
							predicate = (<>9__1 = ((DrugPolicyEntry x) => x.drug == thingDefs[i]));
						}
						if (list.Any(predicate))
						{
							goto IL_C2;
						}
					}
					this.entriesInt.Add(new DrugPolicyEntry
					{
						drug = thingDefs[i],
						allowedForAddiction = true
					});
				}
				IL_C2:
				j = i;
			}
			this.entriesInt.SortBy((DrugPolicyEntry e) => e.drug.GetCompProperties<CompProperties_Drug>().listOrder);
		}

		// Token: 0x060089AE RID: 35246 RVA: 0x0031DBA4 File Offset: 0x0031BDA4
		public override void CopyFrom(Policy other)
		{
			DrugPolicy drugPolicy = other as DrugPolicy;
			if (drugPolicy == null)
			{
				return;
			}
			this.sourceDef = drugPolicy.sourceDef;
			this.entriesInt.Clear();
			foreach (DrugPolicyEntry other2 in drugPolicy.entriesInt)
			{
				DrugPolicyEntry drugPolicyEntry = new DrugPolicyEntry();
				drugPolicyEntry.CopyFrom(other2);
				this.entriesInt.Add(drugPolicyEntry);
			}
		}

		// Token: 0x060089AF RID: 35247 RVA: 0x0031DC2C File Offset: 0x0031BE2C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<DrugPolicyEntry>(ref this.entriesInt, "drugs", LookMode.Deep, Array.Empty<object>());
			Scribe_Defs.Look<DrugPolicyDef>(ref this.sourceDef, "sourceDef");
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.entriesInt != null)
			{
				if (this.entriesInt.RemoveAll((DrugPolicyEntry x) => x == null || x.drug == null) != 0)
				{
					Log.Error("Some DrugPolicyEntries were null after loading.");
				}
				this.InitializeIfNeeded(false);
			}
		}

		// Token: 0x040054CF RID: 21711
		public DrugPolicyDef sourceDef;

		// Token: 0x040054D0 RID: 21712
		private List<DrugPolicyEntry> entriesInt;
	}
}
