<template>
  <div class="widget-params-form">
    <form v-on="$listeners">
      <table>
        <tr v-for="(param, index) in params" :key="index">
          <td>
            <label :for="param.name">{{ param.name }}</label>
          </td>
          <td>
            <input
              type="string"
              v-if="param.type === 'string'"
              :id="param.name"
              :name="param.name"
              :value="param.value"
            />
            <input
              type="number"
              v-else-if="param.type === 'integer'"
              :id="param.name"
              :name="param.name"
              :value="param.value"
            />
            <input
              type="checkbox"
              v-else-if="param.type === 'boolean'"
              :id="param.name"
              :name="param.name"
              :checked="param.value"
            />
            <select
              v-else-if="param.type === 'enum'"
              :id="param.name"
              :name="param.name"
              :value="param.value"
            >
              <option
                v-for="(option, index) in param.allowed_values"
                :key="index"
                :value="option.value"
              >
                {{ option.display_name }}
              </option>
            </select>
          </td>
        </tr>
      </table>

      <button class="gradient save-button" type="submit">Save</button>
    </form>
  </div>
</template>

<script>
export default {
  name: "widget-params-form",
  props: {
    params: Array
  }
}
</script>

<style lang="scss" scoped>
.widget-params-form {
  table {
    td {
      padding: 0.5rem 1rem;
    }
  }

  label {
    font-weight: bolder;
  }

  form {
    select,
    input {
      border-radius: 0;
      box-shadow: none;
      border-bottom: 1px solid #777;
      padding: 0;
      margin: 0;
      font: inherit;
      padding: 0.2rem 0.7rem;
      background: inherit;
    }

    .save-button {
      margin-top: 1rem;
      float: right;
    }
  }
}
</style>
