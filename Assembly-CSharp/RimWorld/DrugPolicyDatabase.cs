using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001020 RID: 4128
	public class DrugPolicyDatabase : IExposable
	{
		// Token: 0x170011D9 RID: 4569
		// (get) Token: 0x060067EE RID: 26606 RVA: 0x0024AF2A File Offset: 0x0024912A
		public List<DrugPolicy> AllPolicies
		{
			get
			{
				return this.policies;
			}
		}

		// Token: 0x060067EF RID: 26607 RVA: 0x0024AF32 File Offset: 0x00249132
		public DrugPolicyDatabase()
		{
			this.GenerateStartingDrugPolicies();
		}

		// Token: 0x060067F0 RID: 26608 RVA: 0x0024AF4B File Offset: 0x0024914B
		public void ExposeData()
		{
			Scribe_Collections.Look<DrugPolicy>(ref this.policies, "policies", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x060067F1 RID: 26609 RVA: 0x0024AF63 File Offset: 0x00249163
		public DrugPolicy DefaultDrugPolicy()
		{
			if (this.policies.Count == 0)
			{
				this.MakeNewDrugPolicy();
			}
			return this.policies[0];
		}

		// Token: 0x060067F2 RID: 26610 RVA: 0x0024AF88 File Offset: 0x00249188
		public void SetDefault(DrugPolicy policy)
		{
			int index = this.policies.IndexOf(policy);
			DrugPolicy value = this.policies[0];
			this.policies[0] = policy;
			this.policies[index] = value;
		}

		// Token: 0x060067F3 RID: 26611 RVA: 0x0024AFCC File Offset: 0x002491CC
		public void MakePolicyDefault(DrugPolicyDef policyDef)
		{
			if (this.DefaultDrugPolicy().sourceDef == policyDef)
			{
				return;
			}
			DrugPolicy drugPolicy = this.policies.FirstOrDefault((DrugPolicy x) => x.sourceDef == policyDef);
			if (drugPolicy != null)
			{
				this.policies.Remove(drugPolicy);
				this.policies.Insert(0, drugPolicy);
			}
		}

		// Token: 0x060067F4 RID: 26612 RVA: 0x0024B030 File Offset: 0x00249230
		public AcceptanceReport TryDelete(DrugPolicy policy)
		{
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive)
			{
				if (pawn.drugs != null && pawn.drugs.CurrentPolicy == policy)
				{
					return new AcceptanceReport("DrugPolicyInUse".Translate(pawn));
				}
			}
			foreach (Pawn pawn2 in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
			{
				if (pawn2.drugs != null && pawn2.drugs.CurrentPolicy == policy)
				{
					pawn2.drugs.CurrentPolicy = null;
				}
			}
			this.policies.Remove(policy);
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x060067F5 RID: 26613 RVA: 0x0024B120 File Offset: 0x00249320
		public DrugPolicy MakeNewDrugPolicy()
		{
			int num;
			if (!this.policies.Any<DrugPolicy>())
			{
				num = 1;
			}
			else
			{
				num = this.policies.Max((DrugPolicy o) => o.id) + 1;
			}
			int id = num;
			DrugPolicy drugPolicy = new DrugPolicy(id, "DrugPolicy".Translate() + " " + id.ToString());
			this.policies.Add(drugPolicy);
			return drugPolicy;
		}

		// Token: 0x060067F6 RID: 26614 RVA: 0x0024B1A4 File Offset: 0x002493A4
		public DrugPolicy NewDrugPolicyFromDef(DrugPolicyDef def)
		{
			DrugPolicy drugPolicy = this.MakeNewDrugPolicy();
			drugPolicy.label = def.LabelCap;
			drugPolicy.sourceDef = def;
			if (def.allowPleasureDrugs)
			{
				for (int i = 0; i < drugPolicy.Count; i++)
				{
					if (drugPolicy[i].drug.IsPleasureDrug)
					{
						drugPolicy[i].allowedForJoy = true;
					}
				}
			}
			if (def.entries != null)
			{
				for (int j = 0; j < def.entries.Count; j++)
				{
					drugPolicy[def.entries[j].drug].CopyFrom(def.entries[j]);
				}
			}
			return drugPolicy;
		}

		// Token: 0x060067F7 RID: 26615 RVA: 0x0024B250 File Offset: 0x00249450
		private void GenerateStartingDrugPolicies()
		{
			foreach (DrugPolicyDef def in DefDatabase<DrugPolicyDef>.AllDefs)
			{
				this.NewDrugPolicyFromDef(def);
			}
		}

		// Token: 0x0400424A RID: 16970
		private List<DrugPolicy> policies = new List<DrugPolicy>();
	}
}
