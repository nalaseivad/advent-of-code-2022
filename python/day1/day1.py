import sys
import re
import pprint


debug = 1

def debug_print(s):
  if debug:
    print(s)

#
# Print a more readable version of our maps
#
def debug_pretty_print(x):
  if debug:
    pp = pprint.PrettyPrinter(indent=4)
    pp.pprint(x)


#
# Split a list into an iterable of sublists based on a test of whether a given list element is a separator
# Examples
#   [1, 2, sep, 3, sep, sep, 4, 5, 6] -> [[1, 2], [3], [], [4, 5, 6]]
#
def split_list(iterable, separator_fn):
  list = []
  for item in iterable:
    if separator_fn(item):
      yield list
      list.clear()
      continue
    list.append(item)
  yield list


def is_blank(line):
  return re.match(r'^\s*$', line)


def part1():
  with open(file_path, 'r') as lines:
    lines = (line.rstrip('\n') for line in lines)
    block_totals = (sum(map(int, block)) for block in split_list(lines, is_blank))
    print(max(block_totals))
  

def part2():
  with open(file_path, 'r') as lines:
    lines = (line.rstrip('\n') for line in lines)
    block_totals = (sum(map(int, block)) for block in split_list(lines, is_blank))
    block_totals_desc = sorted(block_totals, reverse=True)
    print(sum(block_totals_desc[:3]))


if len(sys.argv) != 3:
  print(f'Usage: python3 {sys.argv[0]} <part> <file_path>')
  exit(1)

part = sys.argv[1]
file_path = sys.argv[2]

if part == '1':
  part1()
elif part == '2':
  part2()
else:
  print('Unknown part')
  exit(1)
