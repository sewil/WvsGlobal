import sys
import pathlib
import base64
import glob, os
import xml.etree.ElementTree as ET
import shutil
from distutils.util import strtobool
import re

# Load XML
def get_node_values(node):
	item_id = node.get('name')
	item_name = ''
	for string in node.findall("string"):
		if string.get('name') == 'name':
			item_name = string.get('value')
			return (item_name, item_id)
	return (None, None)

def get_npc_name(root, npc_id):
	table = {}
	for node in root.findall('imgdir'):
		id = re.match(r'^\d+$', node.get('name'))
		# print(node.get('name'), id)
		if id.group(0) == npc_id:
			(node_name, node_id) = get_node_values(node)
			return node_name
	return None

def get_item_names(root):
	table = {}
	for node in root.findall('imgdir'):
		# print(node.get('name'))
		for subnode in node.findall("imgdir"):
			# print(subnode.get('name'))
			if re.match(r'^\d+$', subnode.get('name')):
				(node_name, node_id) = get_node_values(subnode)
				table[node_name] = node_id
			else:
				for item in subnode.findall("imgdir"):
					(node_name, node_id) = get_node_values(item)
					table[node_name] = node_id
	return table

item_tree = ET.parse('Item.img.xml')
item_names = get_item_names(item_tree.getroot())

npc_tree = ET.parse('Npc.img.xml')


shop = '''using System.Linq;
using WvsBeta.Game;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    class Shop
    {
        public ShopItemData[] Rechargables = new ShopItemData[]
        {
            new ShopItemData(2070000),
            new ShopItemData(2070001),
            new ShopItemData(2070002),
            new ShopItemData(2070003),
            new ShopItemData(2070004),
            new ShopItemData(2070005),
            new ShopItemData(2070006),
            new ShopItemData(2070007),
            new ShopItemData(2070008),
            new ShopItemData(2070009),
            new ShopItemData(2070010),
            new ShopItemData(2070011),
            new ShopItemData(2070012),
            new ShopItemData(2070013),
        };
    }
'''

for shop_file in glob.glob("unconverted/*.txt"):
	fname, ext = os.path.splitext(os.path.basename(shop_file))
	fspl = fname.split("_", 1)
	npc_id = fspl[0]
	class_name = fspl[1]
	npc_name = get_npc_name(npc_tree.getroot(), npc_id)
	print(npc_id, class_name, npc_name)

	shop += f'    // {npc_name}\n'
	shop += f'    [Script("{npc_id}")]\n'
	shop += f'    class {class_name} : Shop, INpcScript\n'
	shop += """    {
        public void Run(INpcHost self, GameCharacter target)
        {
            self.AskShop(new ShopItemData[] {
"""

	for line in open(shop_file, 'r').readlines():
		item_name = re.match(r'^[a-zA-Z-\.\d \'\/]+', line).group(0).strip()
		# print(line, re.match(r'^[a-zA-Z- \']+', line))
		mesos = re.search(r'([\d,]+) mesos', line).group(1).replace(",","")
		item_id = item_names[item_name]
		shop += "                new ShopItemData(" + item_id + ", " + mesos + "),\n"

	shop += """            }.Concat(Rechargables));
        }
    }
"""

shop += "}\n"

# print(shop)
os.remove(f'shop.cs') if os.path.exists(f'shop.cs') else None
f = open(f'shop.cs', 'a')
f.writelines(shop)
f.close()
exit()
