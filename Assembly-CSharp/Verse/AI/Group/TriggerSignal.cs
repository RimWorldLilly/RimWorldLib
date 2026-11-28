using System;
using System.Text;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000967 RID: 2407
	public struct TriggerSignal
	{
		// Token: 0x17000D72 RID: 3442
		// (get) Token: 0x06004970 RID: 18800 RVA: 0x001CA5C2 File Offset: 0x001C87C2
		public Pawn Pawn
		{
			get
			{
				return this.thing as Pawn;
			}
		}

		// Token: 0x06004971 RID: 18801 RVA: 0x001CA5D0 File Offset: 0x001C87D0
		public TriggerSignal(TriggerSignalType type)
		{
			this.type = type;
			this.memo = null;
			this.signal = default(Signal);
			this.thing = null;
			this.otherThing = null;
			this.dinfo = default(DamageInfo);
			this.condition = PawnLostCondition.Undefined;
			this.faction = null;
			this.clamorType = null;
			this.previousRelationKind = null;
		}

		// Token: 0x17000D73 RID: 3443
		// (get) Token: 0x06004972 RID: 18802 RVA: 0x001CA632 File Offset: 0x001C8832
		public static TriggerSignal ForTick
		{
			get
			{
				return new TriggerSignal(TriggerSignalType.Tick);
			}
		}

		// Token: 0x06004973 RID: 18803 RVA: 0x001CA63C File Offset: 0x001C883C
		public static TriggerSignal ForMemo(string memo)
		{
			return new TriggerSignal(TriggerSignalType.Memo)
			{
				memo = memo
			};
		}

		// Token: 0x06004974 RID: 18804 RVA: 0x001CA65C File Offset: 0x001C885C
		public static TriggerSignal ForSignal(Signal signal)
		{
			return new TriggerSignal(TriggerSignalType.Signal)
			{
				signal = signal
			};
		}

		// Token: 0x06004975 RID: 18805 RVA: 0x001CA67C File Offset: 0x001C887C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("(");
			stringBuilder.Append(this.type.ToString());
			if (this.memo != null)
			{
				stringBuilder.Append(", memo=" + this.memo);
			}
			if (this.Pawn != null)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				string str = ", pawn=";
				Pawn pawn = this.Pawn;
				stringBuilder2.Append(str + ((pawn != null) ? pawn.ToString() : null));
			}
			if (this.dinfo.Def != null)
			{
				StringBuilder stringBuilder3 = stringBuilder;
				string str2 = ", dinfo=";
				DamageInfo damageInfo = this.dinfo;
				stringBuilder3.Append(str2 + damageInfo.ToString());
			}
			if (this.condition != PawnLostCondition.Undefined)
			{
				stringBuilder.Append(", condition=" + this.condition.ToString());
			}
			if (this.signal.tag != null)
			{
				StringBuilder stringBuilder4 = stringBuilder;
				string str3 = ", signal=";
				Signal signal = this.signal;
				stringBuilder4.Append(str3 + signal.ToString());
			}
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}

		// Token: 0x04002F20 RID: 12064
		public TriggerSignalType type;

		// Token: 0x04002F21 RID: 12065
		public string memo;

		// Token: 0x04002F22 RID: 12066
		public Signal signal;

		// Token: 0x04002F23 RID: 12067
		public Thing thing;

		// Token: 0x04002F24 RID: 12068
		public Thing otherThing;

		// Token: 0x04002F25 RID: 12069
		public DamageInfo dinfo;

		// Token: 0x04002F26 RID: 12070
		public PawnLostCondition condition;

		// Token: 0x04002F27 RID: 12071
		public Faction faction;

		// Token: 0x04002F28 RID: 12072
		public FactionRelationKind? previousRelationKind;

		// Token: 0x04002F29 RID: 12073
		public ClamorDef clamorType;
	}
}
