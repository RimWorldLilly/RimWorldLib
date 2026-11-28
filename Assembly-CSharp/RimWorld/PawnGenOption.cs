using System;
using System.Runtime.CompilerServices;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F54 RID: 3924
	public class PawnGenOption
	{
		// Token: 0x1700113E RID: 4414
		// (get) Token: 0x060064FB RID: 25851 RVA: 0x00240616 File Offset: 0x0023E816
		public float Cost
		{
			get
			{
				return this.kind.combatPower;
			}
		}

		// Token: 0x060064FC RID: 25852 RVA: 0x00240624 File Offset: 0x0023E824
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 3);
			defaultInterpolatedStringHandler.AppendLiteral("(");
			defaultInterpolatedStringHandler.AppendFormatted((this.kind != null) ? this.kind.ToString() : "null");
			defaultInterpolatedStringHandler.AppendLiteral(" w=");
			defaultInterpolatedStringHandler.AppendFormatted<float>(this.selectionWeight, "F2");
			defaultInterpolatedStringHandler.AppendLiteral(" c=");
			defaultInterpolatedStringHandler.AppendFormatted((this.kind != null) ? this.Cost.ToString("F2") : "null");
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x060064FD RID: 25853 RVA: 0x002406CD File Offset: 0x0023E8CD
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "kind", xmlRoot.Name, null, null, null);
			this.selectionWeight = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x04003AE4 RID: 15076
		public PawnKindDef kind;

		// Token: 0x04003AE5 RID: 15077
		public float selectionWeight;
	}
}
