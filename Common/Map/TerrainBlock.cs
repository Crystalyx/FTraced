using GlLib.Client.API;

namespace GlLib.Common.Map
{
    public abstract class TerrainBlock
    {
        public int id = -1;

        /// <summary>
        ///     name in format : [mod:]block.[blockset.]blockname[.subtype]
        /// </summary>
        /// <returns></returns>
        public abstract string GetName();

        public abstract string GetTextureName(World world, int x, int y);

        public virtual bool RequiresSpecialRenderer(World world, int x, int y)
        {
            return false;
        }

        public virtual IBlockRenderer GetSpecialRenderer(World world, int x, int y)
        {
            return null;
        }
    }
}