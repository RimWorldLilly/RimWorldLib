using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02000D8F RID: 3471
	public struct ThoughtState
	{
		// Token: 0x17001094 RID: 4244
		// (get) Token: 0x06005FAD RID: 24493 RVA: 0x0022D1A5 File Offset: 0x0022B3A5
		public bool Active
		{
			get
			{
				return this.stageIndex != -99999;
			}
		}

		// Token: 0x17001095 RID: 4245
		// (get) Token: 0x06005FAE RID: 24494 RVA: 0x0022D1B7 File Offset: 0x0022B3B7
		public int StageIndex
		{
			get
			{
				return this.stageIndex;
			}
		}

		// Token: 0x17001096 RID: 4246
		// (get) Token: 0x06005FAF RID: 24495 RVA: 0x0022D1BF File Offset: 0x0022B3BF
		public string Reason
		{
			get
			{
				return this.reason;
			}
		}

		// Token: 0x17001097 RID: 4247
		// (get) Token: 0x06005FB0 RID: 24496 RVA: 0x0022D1C7 File Offset: 0x0022B3C7
		public static ThoughtState ActiveDefault
		{
			get
			{
				return ThoughtState.ActiveAtStage(0);
			}
		}

		// Token: 0x17001098 RID: 4248
		// (get) Token: 0x06005FB1 RID: 24497 RVA: 0x0022D1D0 File Offset: 0x0022B3D0
		public static ThoughtState Inactive
		{
			get
			{
				return new ThoughtState
				{
					stageIndex = -99999
				};
			}
		}

		// Token: 0x06005FB2 RID: 24498 RVA: 0x0022D1F4 File Offset: 0x0022B3F4
		public static ThoughtState ActiveAtStage(int stageIndex)
		{
			return new ThoughtState
			{
				stageIndex = stageIndex
			};
		}

		// Token: 0x06005FB3 RID: 24499 RVA: 0x0022D214 File Offset: 0x0022B414
		public static ThoughtState ActiveAtStage(int stageIndex, string reason)
		{
			return new ThoughtState
			{
				stageIndex = stageIndex,
				reason = reason
			};
		}

		// Token: 0x06005FB4 RID: 24500 RVA: 0x0022D23C File Offset: 0x0022B43C
		public static ThoughtState ActiveWithReason(string reason)
		{
			ThoughtState activeDefault = ThoughtState.ActiveDefault;
			activeDefault.reason = reason;
			return activeDefault;
		}

		// Token: 0x06005FB5 RID: 24501 RVA: 0x0022D258 File Offset: 0x0022B458
		public static implicit operator ThoughtState(bool value)
		{
			if (value)
			{
				return ThoughtState.ActiveDefault;
			}
			return ThoughtState.Inactive;
		}

		// Token: 0x06005FB6 RID: 24502 RVA: 0x0022D268 File Offset: 0x0022B468
		public bool ActiveFor(ThoughtDef thoughtDef)
		{
			if (!this.Active)
			{
				return false;
			}
			int num = this.StageIndexFor(thoughtDef);
			return num >= 0 && thoughtDef.stages[num] != null;
		}

		// Token: 0x06005FB7 RID: 24503 RVA: 0x0022D29C File Offset: 0x0022B49C
		public int StageIndexFor(ThoughtDef thoughtDef)
		{
			return Mathf.Min(this.StageIndex, thoughtDef.stages.Count - 1);
		}

		// Token: 0x040035D7 RID: 13783
		private int stageIndex;

		// Token: 0x040035D8 RID: 13784
		private string reason;

		// Token: 0x040035D9 RID: 13785
		private const int InactiveIndex = -99999;
	}
}
