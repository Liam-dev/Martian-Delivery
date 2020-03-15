class Animal:

    def __init__(self, name : str, species : str, age : int):
        self.name : str = name
        self.species : str = species
        self.age : int = age
    
    def hello(self):
        return ("My name is {0} and I am a {1} ".format(self.name, self.species))

bob : Animal = Animal("Bob", "Hippo", 10)
print(bob.hello())
