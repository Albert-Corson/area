const syncPost = (url: string, payload: object) => {
  const form = document.createElement("form")
  document.body.appendChild(form)
  form.method = "POST"
  form.action = url
  for (const key of Object.keys(payload)) {
    const input = document.createElement("input")
    input.type = "hidden"
    input.name = key
    input.value = payload[key]
    form.appendChild(input)
  }
  form.submit()
  document.body.removeChild(form)
}

export default syncPost
