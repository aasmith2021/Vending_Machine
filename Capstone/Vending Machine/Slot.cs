using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public class Slot
    {
        public Slot(string slotId)
        {
            SlotId = slotId;
        }

        public Slot(string slotId, Item item, int quantity = 5)
        {

            SlotId = slotId;
            for (int i = 0; i < quantity; i++)
            {
                ItemStack.Push(item);
            }
        }

        public int Quantity
        {
            get
            {
                return ItemStack.Count;
            }
        }

        public string SlotId { get; }

        public Stack<Item> ItemStack { get; set; } = new Stack<Item>();

        public Item ItemInSlot
        {
            get
            {
                if (ItemStack.Count > 0)
                {
                    return ItemStack.Peek();
                }
                else
                {
                    Item soldOutItem = new UnknownItem("SOLD OUT", 0m, "");
                    return soldOutItem;
                }
            }
        }
    }
}
