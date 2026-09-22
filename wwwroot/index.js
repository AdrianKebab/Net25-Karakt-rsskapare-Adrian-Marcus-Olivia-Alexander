const characterPrompt = document.getElementById("character-prompt");
const createCharactherButton = document.getElementById("create-characther");

createCharactherButton.addEventListener("click", () => {
  createCharacter(characterPrompt);
});

async function createCharacter(input) {
  const response = await fetch("/api/create", {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify({
      input: text.value
    })
  });

  const data = await response.json();
}

//funktion för att hämta alla karaktärer?