using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x020007EE RID: 2030
	public struct SoundInfo
	{
		// Token: 0x17000C71 RID: 3185
		// (get) Token: 0x0600404A RID: 16458 RVA: 0x00199CCA File Offset: 0x00197ECA
		// (set) Token: 0x0600404B RID: 16459 RVA: 0x00199CD2 File Offset: 0x00197ED2
		public bool IsOnCamera { readonly get; private set; }

		// Token: 0x17000C72 RID: 3186
		// (get) Token: 0x0600404C RID: 16460 RVA: 0x00199CDB File Offset: 0x00197EDB
		// (set) Token: 0x0600404D RID: 16461 RVA: 0x00199CE3 File Offset: 0x00197EE3
		public TargetInfo Maker { readonly get; private set; }

		// Token: 0x17000C73 RID: 3187
		// (get) Token: 0x0600404E RID: 16462 RVA: 0x00199CEC File Offset: 0x00197EEC
		// (set) Token: 0x0600404F RID: 16463 RVA: 0x00199CF4 File Offset: 0x00197EF4
		public MaintenanceType Maintenance { readonly get; private set; }

		// Token: 0x17000C74 RID: 3188
		// (get) Token: 0x06004050 RID: 16464 RVA: 0x00199CFD File Offset: 0x00197EFD
		public IEnumerable<KeyValuePair<string, float>> DefinedParameters
		{
			get
			{
				SoundInfo.<get_DefinedParameters>d__18 <get_DefinedParameters>d__ = new SoundInfo.<get_DefinedParameters>d__18(-2);
				<get_DefinedParameters>d__.<>3__<>4__this = this;
				return <get_DefinedParameters>d__;
			}
		}

		// Token: 0x06004051 RID: 16465 RVA: 0x00199D14 File Offset: 0x00197F14
		public static SoundInfo OnCamera(MaintenanceType maint = MaintenanceType.None)
		{
			SoundInfo result = default(SoundInfo);
			result.IsOnCamera = true;
			result.Maintenance = maint;
			result.Maker = TargetInfo.Invalid;
			result.testPlay = false;
			result.volumeFactor = (result.pitchFactor = 1f);
			return result;
		}

		// Token: 0x06004052 RID: 16466 RVA: 0x00199D64 File Offset: 0x00197F64
		public static SoundInfo InMap(TargetInfo maker, MaintenanceType maint = MaintenanceType.None)
		{
			SoundInfo result = default(SoundInfo);
			result.IsOnCamera = false;
			result.Maintenance = maint;
			result.Maker = maker;
			result.testPlay = false;
			result.volumeFactor = (result.pitchFactor = 1f);
			return result;
		}

		// Token: 0x06004053 RID: 16467 RVA: 0x00199DB0 File Offset: 0x00197FB0
		public void SetParameter(string key, float value)
		{
			if (this.parameters == null)
			{
				this.parameters = new Dictionary<string, float>();
			}
			this.parameters[key] = value;
		}

		// Token: 0x06004054 RID: 16468 RVA: 0x00199DD2 File Offset: 0x00197FD2
		public static implicit operator SoundInfo(TargetInfo source)
		{
			return SoundInfo.InMap(source, MaintenanceType.None);
		}

		// Token: 0x06004055 RID: 16469 RVA: 0x00199DDB File Offset: 0x00197FDB
		public static implicit operator SoundInfo(Thing sourceThing)
		{
			return SoundInfo.InMap(sourceThing, MaintenanceType.None);
		}

		// Token: 0x06004056 RID: 16470 RVA: 0x00199DEC File Offset: 0x00197FEC
		public override string ToString()
		{
			string text = null;
			if (this.parameters != null && this.parameters.Count > 0)
			{
				text = "parameters=";
				foreach (KeyValuePair<string, float> keyValuePair in this.parameters)
				{
					text = string.Concat(new string[]
					{
						text,
						keyValuePair.Key.ToString(),
						"-",
						keyValuePair.Value.ToString(),
						" "
					});
				}
			}
			string text2 = null;
			if (this.Maker.HasThing || this.Maker.Cell.IsValid)
			{
				text2 = this.Maker.ToString();
			}
			string text3 = null;
			if (this.Maintenance != MaintenanceType.None)
			{
				text3 = ", Maint=" + this.Maintenance.ToString();
			}
			return string.Concat(new string[]
			{
				"(",
				this.IsOnCamera ? "Camera" : "World from ",
				text2,
				text,
				text3,
				")"
			});
		}

		// Token: 0x04002AB3 RID: 10931
		private Dictionary<string, float> parameters;

		// Token: 0x04002AB4 RID: 10932
		public float volumeFactor;

		// Token: 0x04002AB5 RID: 10933
		public float pitchFactor;

		// Token: 0x04002AB6 RID: 10934
		public bool testPlay;

		// Token: 0x04002AB7 RID: 10935
		public bool forcedPlayOnCamera;
	}
}
