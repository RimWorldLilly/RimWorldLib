using System;
using System.Collections.Generic;
using System.IO;
using Verse;

namespace RimWorld
{
	// Token: 0x02001201 RID: 4609
	public static class IdeoFiles
	{
		// Token: 0x1700136F RID: 4975
		// (get) Token: 0x06007280 RID: 29312 RVA: 0x00281731 File Offset: 0x0027F931
		public static IEnumerable<Ideo> AllIdeosLocal
		{
			get
			{
				return IdeoFiles.ideosLocal;
			}
		}

		// Token: 0x06007281 RID: 29313 RVA: 0x00281738 File Offset: 0x0027F938
		public static void RecacheData()
		{
			IdeoFiles.ideosLocal.Clear();
			using (IEnumerator<FileInfo> enumerator = GenFilePaths.AllCustomIdeoFiles.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Ideo item;
					if (GameDataSaveLoader.TryLoadIdeo(enumerator.Current.FullName, out item))
					{
						IdeoFiles.ideosLocal.Add(item);
					}
				}
			}
		}

		// Token: 0x0400487F RID: 18559
		private static List<Ideo> ideosLocal = new List<Ideo>();
	}
}
