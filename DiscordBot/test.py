import unittest
from utils import get_gm_level

class Role:
     def __init__(self, id: int):
        self.id = id

class TestBot(unittest.TestCase):
    tester = Role(id=1302212616223195166)
    gm_intern = Role(id=1303378190395506728)
    gm = Role(id=1297210415339667456)
    admin = Role(id=1303378104823320706)

    def test_gm_level_tester(self):
        roles = [self.tester]
        expected_gm_level = 16
        actual_gm_level = get_gm_level(roles)
        self.assertEqual(expected_gm_level, actual_gm_level)

    def test_gm_level_gm_intern(self):
        roles = [self.gm_intern]
        expected_gm_level = 32
        actual_gm_level = get_gm_level(roles)
        self.assertEqual(expected_gm_level, actual_gm_level)

    def test_gm_level_gm(self):
        roles = [self.gm]
        expected_gm_level = 64
        actual_gm_level = get_gm_level(roles)
        self.assertEqual(expected_gm_level, actual_gm_level)

    def test_gm_level_admin(self):
        roles = [self.admin]
        expected_gm_level = 128
        actual_gm_level = get_gm_level(roles)
        self.assertEqual(expected_gm_level, actual_gm_level)

    def test_gm_level_gm_tester(self):
        roles = [self.tester, self.gm]
        expected_gm_level = 80
        actual_gm_level = get_gm_level(roles)
        self.assertEqual(expected_gm_level, actual_gm_level)

    def test_gm_level_admin_tester(self):
        roles = [self.tester, self.admin]
        expected_gm_level = 144
        actual_gm_level = get_gm_level(roles)
        self.assertEqual(expected_gm_level, actual_gm_level)

if __name__ == '__main__':
    unittest.main()
