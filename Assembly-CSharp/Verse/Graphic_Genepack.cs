using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000569 RID: 1385
	public class Graphic_Genepack : Graphic_Collection
	{
		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x06002916 RID: 10518 RVA: 0x000FCAD4 File Offset: 0x000FACD4
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[0].MatSingle;
			}
		}

		// Token: 0x06002917 RID: 10519 RVA: 0x000FCAE3 File Offset: 0x000FACE3
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return GraphicDatabase.Get<Graphic_Genepack>(this.path, newShader, this.drawSize, newColor, newColorTwo, this.data, null);
		}

		// Token: 0x06002918 RID: 10520 RVA: 0x000FCB00 File Offset: 0x000FAD00
		public Graphic SubGraphicFor(Thing thing)
		{
			if (thing != null)
			{
				Genepack genepack = thing as Genepack;
				if (genepack != null && genepack.GeneSet != null)
				{
					using (List<GeneDef>.Enumerator enumerator = genepack.GeneSet.GenesListForReading.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.biostatArc > 0)
							{
								return this.subGraphics[this.subGraphics.Length - 1];
							}
						}
					}
					return this.SubGraphicForGeneCount(genepack.GeneSet.GenesListForReading.Count);
				}
			}
			return this.subGraphics[0];
		}

		// Token: 0x06002919 RID: 10521 RVA: 0x000FCBA4 File Offset: 0x000FADA4
		public override Material MatAt(Rot4 rot, Thing thing = null)
		{
			if (thing == null)
			{
				return this.MatSingle;
			}
			return this.MatSingleFor(thing);
		}

		// Token: 0x0600291A RID: 10522 RVA: 0x000FCBB7 File Offset: 0x000FADB7
		public override Material MatSingleFor(Thing thing)
		{
			return this.SubGraphicFor(thing).MatSingle;
		}

		// Token: 0x0600291B RID: 10523 RVA: 0x000FCBC5 File Offset: 0x000FADC5
		public Graphic SubGraphicForGeneCount(int geneCount)
		{
			geneCount = Mathf.Min(geneCount, 4);
			return this.subGraphics[Mathf.Min(geneCount, this.subGraphics.Length - 1)];
		}

		// Token: 0x04001CC6 RID: 7366
		private const int MaxDisplayedGenes = 4;
	}
}
