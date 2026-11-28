using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x020007FD RID: 2045
	public class SustainerManager
	{
		// Token: 0x17000C7C RID: 3196
		// (get) Token: 0x06004088 RID: 16520 RVA: 0x0019B2D4 File Offset: 0x001994D4
		public List<Sustainer> AllSustainers
		{
			get
			{
				return this.allSustainers;
			}
		}

		// Token: 0x06004089 RID: 16521 RVA: 0x0019B2DC File Offset: 0x001994DC
		public void RegisterSustainer(Sustainer newSustainer)
		{
			this.allSustainers.Add(newSustainer);
		}

		// Token: 0x0600408A RID: 16522 RVA: 0x0019B2EA File Offset: 0x001994EA
		public void DeregisterSustainer(Sustainer oldSustainer)
		{
			this.allSustainers.Remove(oldSustainer);
		}

		// Token: 0x0600408B RID: 16523 RVA: 0x0019B2FC File Offset: 0x001994FC
		public bool SustainerExists(SoundDef def)
		{
			for (int i = 0; i < this.allSustainers.Count; i++)
			{
				if (this.allSustainers[i].def == def)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600408C RID: 16524 RVA: 0x0019B338 File Offset: 0x00199538
		public void SustainerManagerUpdate()
		{
			for (int i = this.allSustainers.Count - 1; i >= 0; i--)
			{
				this.allSustainers[i].SustainerUpdate();
			}
			this.UpdateAllSustainerScopes();
		}

		// Token: 0x0600408D RID: 16525 RVA: 0x0019B374 File Offset: 0x00199574
		public void UpdateAllSustainerScopes()
		{
			SustainerManager.playingPerDef.Clear();
			for (int i = 0; i < this.allSustainers.Count; i++)
			{
				Sustainer sustainer = this.allSustainers[i];
				if (!SustainerManager.playingPerDef.ContainsKey(sustainer.def))
				{
					List<Sustainer> list = SimplePool<List<Sustainer>>.Get();
					list.Add(sustainer);
					SustainerManager.playingPerDef.Add(sustainer.def, list);
				}
				else
				{
					SustainerManager.playingPerDef[sustainer.def].Add(sustainer);
				}
			}
			foreach (KeyValuePair<SoundDef, List<Sustainer>> keyValuePair in SustainerManager.playingPerDef)
			{
				SoundDef key = keyValuePair.Key;
				List<Sustainer> value = keyValuePair.Value;
				if (value.Count - key.maxVoices < 0)
				{
					for (int j = 0; j < value.Count; j++)
					{
						value[j].scopeFader.inScope = true;
					}
				}
				else
				{
					for (int k = 0; k < value.Count; k++)
					{
						value[k].scopeFader.inScope = false;
					}
					value.Sort(SustainerManager.SortSustainersByCameraDistanceCached);
					int num = 0;
					for (int l = 0; l < value.Count; l++)
					{
						value[l].scopeFader.inScope = true;
						num++;
						if (num >= key.maxVoices)
						{
							break;
						}
					}
					for (int m = 0; m < value.Count; m++)
					{
						if (!value[m].scopeFader.inScope)
						{
							value[m].scopeFader.inScopePercent = 0f;
						}
					}
				}
			}
			foreach (KeyValuePair<SoundDef, List<Sustainer>> keyValuePair2 in SustainerManager.playingPerDef)
			{
				keyValuePair2.Value.Clear();
				SimplePool<List<Sustainer>>.Return(keyValuePair2.Value);
			}
			SustainerManager.playingPerDef.Clear();
		}

		// Token: 0x0600408E RID: 16526 RVA: 0x0019B5BC File Offset: 0x001997BC
		public void EndAllInMap(Map map)
		{
			for (int i = this.allSustainers.Count - 1; i >= 0; i--)
			{
				if (this.allSustainers[i].info.Maker.Map == map)
				{
					this.allSustainers[i].End();
				}
			}
		}

		// Token: 0x04002ADF RID: 10975
		private List<Sustainer> allSustainers = new List<Sustainer>();

		// Token: 0x04002AE0 RID: 10976
		private static Dictionary<SoundDef, List<Sustainer>> playingPerDef = new Dictionary<SoundDef, List<Sustainer>>();

		// Token: 0x04002AE1 RID: 10977
		private static readonly Comparison<Sustainer> SortSustainersByCameraDistanceCached = (Sustainer a, Sustainer b) => a.CameraDistanceSquared.CompareTo(b.CameraDistanceSquared);
	}
}
