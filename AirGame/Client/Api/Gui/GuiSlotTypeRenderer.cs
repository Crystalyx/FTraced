using GlLib.Client.Api.Sprites;
using GlLib.Client.Graphic;
using GlLib.Common.Api.Inventory;
using GlLib.Utils;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace GlLib.Client.Api.Gui
{
    public class GuiSlotTypeRenderer : GuiObject
    {
        public IInventory inventory;
        public int slot;

        public Texture selectedTexture;
        public Texture switchTexture;
        public TextureLayout slotTexture;

        public GuiSlotTypeRenderer(IInventory _inventory, int _slot, int _x, int _y) : base(_x, _y, GuiSlot.SlotSize,
            GuiSlot.SlotSize)
        {
            slotTexture = new TextureLayout("gui/item_classes.png", 4, 4);
            selectedTexture = Vertexer.LoadTexture("gui/slot_selected.png");
            switchTexture = Vertexer.LoadTexture("gui/slot_switch.png");
            inventory = _inventory;
            slot = _slot;
        }

        public GuiSlotTypeRenderer(IInventory _inventory, int _slot, int _x, int _y, Color _color) : base(
            _x, _y, GuiSlot.SlotSize, GuiSlot.SlotSize, _color)
        {
            slotTexture = new TextureLayout("gui/item_classes.png", 4, 4);
            selectedTexture = Vertexer.LoadTexture("gui/slot_selected.png");
            switchTexture = Vertexer.LoadTexture("gui/slot_switch.png");
            inventory = _inventory;
            slot = _slot;
        }

        public override GuiObject OnMouseClick(GuiFrame _gui, MouseButton _button, int _mouseX, int _mouseY)
        {
            if (_button == MouseButton.Right)
            {
                inventory.SelectSlot(slot);
            }
            else
            {
                if (_gui.focusedObject is GuiSlotTypeRenderer gstr)
                {
                    var slotStack = inventory.GetStackInSlot(slot);
                    var selectedStack = gstr.inventory.GetStackInSlot(gstr.slot);
                    if (slotStack != selectedStack && inventory.IsItemValidForSlot(selectedStack, slot) &&
                        gstr.inventory.IsItemValidForSlot(slotStack, gstr.slot))
                    {
                        if (inventory is InventoryList il)
                            il.EnableNull = true;
                        if (gstr.inventory is InventoryList gil)
                            gil.EnableNull = true;
                        inventory.RemoveItemStack(slot);
                        gstr.inventory.RemoveItemStack(gstr.slot);
                        inventory.SetItemStack(selectedStack, slot);
                        gstr.inventory.SetItemStack(slotStack, gstr.slot);
                        if (inventory is InventoryList il2)
                            il2.EnableNull = false;
                        if (gstr.inventory is InventoryList gil2)
                            gil2.EnableNull = false;
                    }

                    return null;
                }
            }

            return this;
        }

        public override bool UnfocusOnRelease()
        {
            return false;
        }

        public override void Render(GuiFrame _gui, int _centerX, int _centerY)
        {
            slotTexture.texture.Bind();
            if (inventory.GetStackInSlot(slot) != null)
            {
                GL.PushMatrix();
                GL.Translate(x + GuiSlot.SlotSize / 2, y + GuiSlot.SlotSize / 2, 0);
                inventory.GetStackInSlot(slot).item.GetItemSprite(inventory.GetStackInSlot(slot)).Render();
//                Vertexer.DrawLayoutPart(slotTexture, x, y, (int) inventory.GetStackInSlot(slot).item.type,
//                    width, height);
                GL.PopMatrix();
            }

            if (slot == inventory.GetSelectedSlot())
            {
                selectedTexture.Bind();
                Vertexer.DrawSquare(x, y, x + width, y + height);
            }

            if (_gui.focusedObject == this)
            {
                switchTexture.Bind();
                Vertexer.DrawSquare(x, y, x + width, y + height);
            }
        }
    }
}