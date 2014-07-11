import json

# dict to json
d=dict(name="cui",age=20,score=88)
print json.dumps(d)

#list to json
l=["cui",20,88]
print json.dumps(l)

#object to json
class Student(object):
	"""docstring for Student"""
	def __init__(self):
		super(Student, self).__init__()
		self.age=20
		self.name="cui"
		self.score=88

print json.dumps(Student(),default=lambda obj:obj.__dict__)

#json to dict
json_str='{"age": 20, "score": 88, "name": "cui"}'	
d= json.loads(json_str)
print d

#json to list
json_str='["cui", 20, 88]'
l=json.loads(json_str)
print l

#json to object
json_str='{"age": 20, "score": 88, "name": "cui"}'
def dict2Student(d):
	s=Student()
	s.name=d["name"]
	s.age=d["age"]
	s.score=d["score"]
	return s

student=json.loads(json_str,object_hook=dict2Student)
print student