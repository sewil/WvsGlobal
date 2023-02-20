import sys
import pathlib
import base64
import os
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

line_idx = -1
for line in open('raw.txt', 'r').readlines():
	line_idx+=1
	if line_idx == 0:
		npc_id = line.splitlines()[0]
		npc_name = get_npc_name(npc_tree.getroot(), npc_id)
		print(npc_id, npc_name)
		class_name = re.sub(r'[\.\s]+', '_', npc_name)

		shop = f'	// {npc_name}\n'
		shop += f'	[Script("{npc_id}")]\n'
		shop += f'	class {class_name} : Shop, INpcScript\n'
		shop += """	{
				public void Run(INpcHost self, GameCharacter target)
				{
					self.AskShop(new ShopItemData[] {
"""
		continue
	item_name = re.match(r'^[a-zA-Z- \'\/]+', line).group(0).strip()
	# print(line, re.match(r'^[a-zA-Z- \']+', line))
	mesos = re.search(r'([\d,]+) mesos', line).group(1).replace(",","")
	item_id = item_names[item_name]
	shop += "						new ShopItemData(" + item_id + ", " + mesos + "),\n"

shop += """			}.Concat(Rechargables));
		}
	}
"""

print(shop)
os.remove(f'{npc_id}.cs') if os.path.exists(f'{npc_id}.cs') else None
f = open(f'{npc_id}.cs', 'a')
f.writelines(shop)
f.close()
exit()
