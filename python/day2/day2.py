import sys
import re
import pprint


debug = 0

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


round_result_map_1 = {
  'A' : {         # Rock
    'X' : 1 + 3,    # Rock      -> Draw
    'Y' : 2 + 6,    # Paper     -> Win
    'Z' : 3 + 0     # Scissors  -> Lose
  },
  'B' : {         # Paper
    'X' : 1 + 0,    # Rock      -> Lose
    'Y' : 2 + 3,    # Paper     -> Draw
    'Z' : 3 + 6     # Scissors  -> Win
  },
  'C' : {         # Scissors
    'X' : 1 + 6,    # Rock      -> Win
    'Y' : 2 + 0,    # Paper     -> Lose
    'Z' : 3 + 3     # Scissors  -> Draw
  }
}

round_result_map_2 = {
  'A' : {         # Rock
    'X' : 3 + 0,    # Lose  -> Scissors
    'Y' : 1 + 3,    # Draw  -> Rock
    'Z' : 2 + 6     # Win   -> Paper
  },
  'B' : {         # Paper
    'X' : 1 + 0,    # Lose  -> Rock
    'Y' : 2 + 3,    # Draw  -> Paper
    'Z' : 3 + 6     # Win   -> Scissors
  },
  'C' : {         # Scissors
    'X' : 2 + 0,    # Lose  -> Paper
    'Y' : 3 + 3,    # Draw  -> Scissors
    'Z' : 1 + 6     # Win   -> Rock
  }
}


def part_n(file_path, round_result_map):
  with open(file_path, 'r') as lines:
    rounds = (line.rstrip('\n') for line in lines)
    round_throws = (re.split(r'\s+', round) for round in rounds)
    print(sum(round_result_map[them][me] for them, me in round_throws))


def part_1(file_path):
  part_n(file_path, round_result_map_1)
  

def part_2(file_path):
  part_n(file_path, round_result_map_2)


if len(sys.argv) != 3:
  print(f'Usage: python3 {sys.argv[0]} <part> <file_path>')
  exit(1)

part = sys.argv[1]
file_path = sys.argv[2]

if part == '1':
  part_1(file_path)
elif part == '2':
  part_2(file_path)
else:
  print('Unknown part')
  exit(1)
