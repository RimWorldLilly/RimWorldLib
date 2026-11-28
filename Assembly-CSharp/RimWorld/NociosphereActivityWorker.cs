using System;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020017E8 RID: 6120
	public class NociosphereActivityWorker : PawnActivityWorker
	{
		// Token: 0x060098AF RID: 39087 RVA: 0x003753EC File Offset: 0x003735EC
		public override float GetChangeRatePerDay(ThingWithComps thing)
		{
			CompNociosphere comp = thing.GetComp<CompNociosphere>();
			float num = base.GetChangeRatePerDay(thing);
			if (comp.IsUnstable)
			{
				num += -0.05f;
			}
			return num;
		}

		// Token: 0x060098B0 RID: 39088 RVA: 0x00375418 File Offset: 0x00373618
		public override void GetSummary(ThingWithComps thing, StringBuilder sb)
		{
			CompNociosphere comp = thing.GetComp<CompNociosphere>();
			base.GetSummary(thing, sb);
			if (comp.IsUnstable)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 2);
				defaultInterpolatedStringHandler.AppendLiteral("\n - ");
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("Unstable".Translate());
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted(-0.05f.ToStringPercent("0"));
				sb.Append(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x04005B3A RID: 23354
		private const float UnstableChangePerDay = -0.05f;
	}
}
