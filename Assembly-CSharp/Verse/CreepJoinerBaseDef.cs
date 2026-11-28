using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000126 RID: 294
	public class CreepJoinerBaseDef : Def, ICreepJoinerDef
	{
		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060009C1 RID: 2497 RVA: 0x00036877 File Offset: 0x00034A77
		public float Weight
		{
			get
			{
				return this.weight;
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060009C2 RID: 2498 RVA: 0x0003687F File Offset: 0x00034A7F
		public List<CreepJoinerBaseDef> Excludes
		{
			get
			{
				return this.excludes;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060009C3 RID: 2499 RVA: 0x00036887 File Offset: 0x00034A87
		public List<CreepJoinerBaseDef> Requires
		{
			get
			{
				return this.requires;
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060009C4 RID: 2500 RVA: 0x0003688F File Offset: 0x00034A8F
		public float MinCombatPoints
		{
			get
			{
				return this.minCombatPoints;
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060009C5 RID: 2501 RVA: 0x00036897 File Offset: 0x00034A97
		public bool CanOccurRandomly
		{
			get
			{
				return this.canOccurRandomly;
			}
		}

		// Token: 0x04000507 RID: 1287
		private float weight = 1f;

		// Token: 0x04000508 RID: 1288
		private float minCombatPoints;

		// Token: 0x04000509 RID: 1289
		private bool canOccurRandomly = true;

		// Token: 0x0400050A RID: 1290
		private List<CreepJoinerBaseDef> excludes = new List<CreepJoinerBaseDef>();

		// Token: 0x0400050B RID: 1291
		private List<CreepJoinerBaseDef> requires = new List<CreepJoinerBaseDef>();

		// Token: 0x0400050C RID: 1292
		[MustTranslate]
		public string surgicalInspectionLetterExtra;
	}
}
