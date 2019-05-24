﻿using GlLib.Client.Api.Sprites;
using GlLib.Client.API;
using GlLib.Common.Entities;
using GlLib.Utils;
using OpenTK.Graphics;

namespace GlLib.Client.Graphic.Renderers
{
    internal class BatRenderer : EntityRenderer
    {
        public LinearSprite batSprite;
        // TODO 
        public LinearSprite spawnSprite;

        public override void Setup(Entity _p)
        {
            var layout = new TextureLayout("bat.png", 6, 1);
            batSprite = new LinearSprite(layout, 6, 6);
            var box = _p.AaBb;
            batSprite.Scale((float) box.Width, (float) box.Height);
            spawnSprite = SpawnSprite;
            spawnSprite.Translate(new PlanarVector(-2, 40));
            spawnSprite.SetColor(new Color4(1, 1, 1, 0.5f));
        }

        public override void Render(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis)
        {
            batSprite.Render();
            if (spawnSprite.FullFrameCount < 1)
                spawnSprite.Render();
        }
    }
}