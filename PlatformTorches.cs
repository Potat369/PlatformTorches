using System;
using System.Reflection;
using MonoMod.Cil;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace PlatformTorches
{
	public class PlatformTorches : Mod
	{
		public override void Load()
		{
			IL_SmartCursorHelper.Step_Torch += HookStepTorch;
		}

		private void HookStepTorch(ILContext il)
		{
			try
			{
				var c = new ILCursor(il);
				
				if (c.TryGotoNext(i => i.MatchLdsfld(typeof(Main).GetField("tileNoAttach")!), i => i.MatchLdloca(10)))
				{
					c.EmitLdsfld(typeof(TileID.Sets).GetField("Platforms", BindingFlags.Public | BindingFlags.Static)!);
					c.EmitLdloca(10);
					c.EmitCall(typeof(Tile).GetProperty("type", BindingFlags.NonPublic | BindingFlags.Instance)!.GetGetMethod(true)!);
					c.EmitLdindU2();
					c.EmitLdelemU1();
					c.EmitBrtrue(c.Instrs[c.Index + 6]);
				}
			}
			catch
			{
				MonoModHooks.DumpIL(ModContent.GetInstance<PlatformTorches>(), il);
			}
		}
	}
}
