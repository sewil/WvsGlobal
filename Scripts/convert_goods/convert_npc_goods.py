import sys
import pathlib
import base64
import os
import xml.etree.ElementTree as ET
import shutil
from distutils.util import strtobool
import re

# Load XML
tree = ET.parse('Item.img.xml')
root = tree.getroot()

def find_item_id(item_name):
	item_id = item_names[item_name]
	return item_id

def get_item_values(item):
	item_id = item.get('name')
	item_name = ''
	for string in item.findall("string"):
		if string.get('name') == 'name':
			item_name = string.get('value')
			return (item_name, item_id)
	return (None, None)

def get_item_names():
	table = {}
	for item_category in root.findall('imgdir'):
		# print(item_category.get('name'))
		for subnode in item_category.findall("imgdir"):
			if re.match(r'^\d+$', subnode.get('name')):
				(item_name, item_id) = get_item_values(subnode)
				table[item_name] = item_id
			else:
				# print(subnode.get('name'))
				for item in subnode.findall("imgdir"):
					(item_name, item_id) = get_item_values(item)
					table[item_name] = item_id
	return table

item_names = get_item_names()

goods = ""
for line in open('raw.txt', 'r').readlines():
	item_name = re.match(r'^[a-zA-Z- \']+', line).group(0).strip()
	# print(line, re.match(r'^[a-zA-Z- \']+', line))
	mesos = re.search(r'(\d+) mesos', line).group(1)
	item_id = find_item_id(item_name)
	goods += "new ShopItemData { ItemID = " + item_id + ", Price = " + mesos + " },\n"

print(goods)
os.remove('goods.txt') if os.path.exists('goods.txt') else None
f = open('goods.txt', 'a')
f.writelines(goods)
f.close()
exit()
