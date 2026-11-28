using System;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020020C3 RID: 8387
	public abstract class SketchResolver
	{
		// Token: 0x0600D338 RID: 54072 RVA: 0x004DBC18 File Offset: 0x004D9E18
		public void Resolve(SketchResolveParams parms)
		{
			try
			{
				this.ResolveInt(parms);
			}
			catch (Exception ex)
			{
				string[] array = new string[6];
				array[0] = "Exception resolving ";
				array[1] = base.GetType().Name;
				array[2] = ": ";
				int num = 3;
				Exception ex2 = ex;
				array[num] = ((ex2 != null) ? ex2.ToString() : null);
				array[4] = "\n\nParms:\n";
				array[5] = parms.ToString();
				Log.Error(string.Concat(array));
			}
		}

		// Token: 0x0600D339 RID: 54073 RVA: 0x004DBC98 File Offset: 0x004D9E98
		public bool CanResolve(SketchResolveParams parms)
		{
			bool result;
			try
			{
				result = this.CanResolveInt(parms);
			}
			catch (Exception ex)
			{
				string[] array = new string[6];
				array[0] = "Exception test running ";
				array[1] = base.GetType().Name;
				array[2] = ": ";
				int num = 3;
				Exception ex2 = ex;
				array[num] = ((ex2 != null) ? ex2.ToString() : null);
				array[4] = "\n\nParms:\n";
				array[5] = parms.ToString();
				Log.Error(string.Concat(array));
				result = false;
			}
			return result;
		}

		// Token: 0x0600D33A RID: 54074
		protected abstract void ResolveInt(SketchResolveParams parms);

		// Token: 0x0600D33B RID: 54075
		protected abstract bool CanResolveInt(SketchResolveParams parms);
	}
}
