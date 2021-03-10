import {Text, TouchableOpacity} from 'react-native'
import React from 'react'

import {render, unmountComponentAtNode} from 'react-dom'
import {act} from 'react-dom/test-utils'

import FlatButton from '../../src/Components/FlatButton'

import renderer from 'react-test-renderer'

let container = null
beforeEach(() => {
  // met en place un élément DOM comme cible de rendu
  container = document.createElement('div')
  document.body.appendChild(container)
})

afterEach(() => {
  // nettoie en sortie de test
  unmountComponentAtNode(container)
  container.remove()
  container = null
})

it('Button value is correctly displayed', () => {
  const rendered = renderer.create(< FlatButton onPress={() => {}} value={'I\'m a very nice value'} />)
  const instance = rendered.root

  expect(instance.findByType((<Text />).type).props.children).toBe('I\'m a very nice value')
})

it('Button press callback well passed', () => {
  act(() => {
    render(<FlatButton />, container)
  })

  console.log(container)

  expect(container.textContent).toBe('Salut, étranger')
})
